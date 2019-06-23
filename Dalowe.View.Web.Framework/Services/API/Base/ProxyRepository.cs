using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Arwend;
using Arwend.Web.Application.Server;
using Dalowe.Data;
using Dalowe.Domain.Visa;
using Newtonsoft.Json;

namespace Dalowe.View.Web.Framework.Services.API.Base
{
    public class ProxyRepository<T> : RealProxy
    {
        private readonly T _decorated;
        private readonly Func<User> _getUserFunc;

        public ProxyRepository(T decorated, Func<User> getUserFunc) : base(typeof(T))
        {
            _decorated = decorated != null ? decorated : throw new ArgumentNullException(nameof(decorated));
            _getUserFunc = getUserFunc ?? throw new ArgumentNullException(nameof(getUserFunc));
        }

        public static ProxyRepository<T> Create(T decorated, Func<User> getUserFunc)
        {
            var proxy = new ProxyRepository<T>(decorated, getUserFunc);
            return proxy;
        }

        public override IMessage Invoke(IMessage methodInvokeMessage)
        {
            if (!(methodInvokeMessage is IMethodCallMessage methodCall))
                throw new ArgumentException(nameof(methodInvokeMessage));

            var targetMethod = methodCall.MethodBase as MethodInfo;
            if (targetMethod == null)
                throw new ArgumentException($"BSM Data Entity could not get {nameof(targetMethod)}");

            var permissionType = typeof(IReadRepository<>).GetMethods().FirstOrDefault(x => x.Name == targetMethod.Name) != null ? PermissionType.Read : PermissionType.Write;

            var currentUser = _getUserFunc.Invoke();
            if (currentUser != null)
            {
                var repositoryName = typeof(T).Name;
                var userPermissions = currentUser.Role?.Permissions;
                var permission = userPermissions?.FirstOrDefault(p => p.Name == "All" && p.Type == permissionType) ?? userPermissions?.FirstOrDefault(p => p.Name == repositoryName && p.Type == permissionType);
                if (permission == null)
                    throw new UnauthorizedAccessException();
            }

            var args = methodCall.Args;

            var cacheEnabled = targetMethod.Name == "GetAll" && ConfigurationManager.CachingEnabled;
            var cacheKey = string.Empty;
            if (cacheEnabled)
            {
                var cacheKeyMethodInfo = GenerateKey(methodCall);
                cacheKey = $"{cacheKeyMethodInfo}";

                var cachedData = CacheManager.Get(cacheKey);
                if (cachedData != null)
                    return new ReturnMessage(cachedData, args, args.Length, methodCall.LogicalCallContext, methodCall);
            }

            var result = targetMethod.Invoke(_decorated, args);
            if (cacheEnabled)
                CacheManager.Add(cacheKey, result, CacheManager.CacheDuration);
            return new ReturnMessage(result, args, args.Length, methodCall.LogicalCallContext, methodCall);
        }

        public string GenerateKey(IMethodCallMessage input)
        {
            string fullMethodName;

            if (input.MethodBase.DeclaringType != null)
                fullMethodName = input.MethodBase.DeclaringType.FullName + "$" + input.MethodBase.Name;
            else
                fullMethodName = input.MethodBase.Name;

            var parameters = input.MethodBase.GetParameters();

            var info = "";

            for (var i = 0; i < parameters.Length; i++)
            {
                var p = parameters[i];
                var pType = p.ParameterType;
                var argument = input.InArgs[i];

                if (IsSimpleType(pType))
                {
                    var o = argument ?? " ";
                    info += pType.FullName + "##" + o + "###";
                }
                else
                {
                    var ser = JsonConvert.SerializeObject(argument);
                    info += pType.FullName + "##" + ser + "###";
                }
            }

            var result = fullMethodName;

            if (string.IsNullOrWhiteSpace(info) == false)
                result += "$" + info.TrimEnd('#');

            return result;
        }

        private bool IsSimpleType(Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new[]
                {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}