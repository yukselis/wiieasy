using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Text.RegularExpressions;
using Dalowe.Data;
using Dalowe.Data.Entity;
using Dalowe.Domain.Base;
using Dalowe.Domain.Visa;

namespace Dalowe.View.Web.Framework.Services.API.Base
{
    public abstract class BaseApi
    {
        private string _sModuleName;

        protected BaseApi(Facade facade)
        {
            Facade = facade;
        }

        internal Client Client => Facade.Services.Client;

        protected Facade Facade { get; }

        public string ModuleName
        {
            get
            {
                if (!string.IsNullOrEmpty(_sModuleName))
                    return _sModuleName;

                var memberInfo = GetType().BaseType;
                if (memberInfo != null) _sModuleName = memberInfo.Name.Replace("Api", "");
                return _sModuleName;
            }
        }

        private Type GetRealType(Type type)
        {
            return typeof(RepositoryFactory).Assembly
                .GetExportedTypes()
                .Where(type.IsAssignableFrom)
                .First(t => !t.IsAbstract && !t.IsInterface);
        }

        private IRepository<TEntity> GetProxyRepository<TEntity>(IRepository<TEntity> result) where TEntity : DbEntity
        {
            var type = GetRealType(result.GetType());

            var proxiedRepositoryType = typeof(ProxyRepository<>).MakeGenericType(type);
            var getUserFunc = new Func<User>(() => Client.CurrentUser);
            var proxiedRepositoryArgs = new object[] { result, getUserFunc };

            var proxyRepository = (RealProxy)proxiedRepositoryType.InvokeMember("Create", BindingFlags.InvokeMethod, null, null, proxiedRepositoryArgs);
            return (IRepository<TEntity>)proxyRepository.GetTransparentProxy();
        }

        protected IRepository<TEntity> Repository<TEntity>() where TEntity : DbEntity
        {
            return GetProxyRepository(RepositoryFactory.Current.GetRepositoryFromEntity<TEntity>());
        }

        internal void Add<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            var entity = dbEntity as Entity;
            if (entity != null)
                entity.UserModifiedID = Client.CurrentUser?.ID;

            dbEntity.UserCreatedID = Client.CurrentUser?.ID;
            var repository = Repository<TEntity>();
            repository.Add(dbEntity);
            repository.SaveChanges();
        }

        internal void Update<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            var entity = dbEntity as Entity;
            if (entity != null)
                entity.UserModifiedID = Client.CurrentUser?.ID;

            dbEntity.UserCreatedID = Client.CurrentUser?.ID;
            var repository = Repository<TEntity>();
            repository.Update(dbEntity);
            repository.SaveChanges();
        }

        internal void Delete<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            var entity = dbEntity as Entity;
            if (entity != null)
                entity.UserModifiedID = Client.CurrentUser?.ID;

            dbEntity.UserCreatedID = Client.CurrentUser?.ID;
            var repository = Repository<TEntity>();
            repository.Delete(dbEntity);
            repository.SaveChanges();
        }

        public string GetResponse(string url)
        {
            HttpWebRequest webRequest;
            var result = string.Empty;
            try
            {
                if (url.IndexOf("http://", StringComparison.InvariantCultureIgnoreCase) == -1 &&
                    url.IndexOf("https://", StringComparison.InvariantCultureIgnoreCase) == -1)
                    url = "http://" + url;
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    webRequest.KeepAlive = false;
                    webRequest.UserAgent =
                        "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)";
                    webRequest.Accept = "text/html;";
                    using (var response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                            using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.GetEncoding("UTF-8")))
                            {
                                result = reader.ReadToEnd();
                                reader.Close();
                            }

                        response.Close();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }

        public void SaveEntity<TEntity>(TEntity dbEntity, bool saveChanges = true) where TEntity : DbEntity
        {
            var datasource = Repository<TEntity>();
            var item = datasource.First(e => e.ID == dbEntity.ID);
            item = dbEntity;

            var entity = item as Entity;
            if (entity != null)
                entity.UserModifiedID = Client.CurrentUser?.ID;

            if (saveChanges)
                datasource.SaveChanges();

            RefreshContext();
        }

        public void DeleteEntity<TEntity>(TEntity entity) where TEntity : DbEntity
        {
            var datasource = Repository<TEntity>();
            datasource.Delete(entity);
            datasource.SaveChanges();
            RefreshContext();
        }

        public TEntity InsertEntity<TEntity>(TEntity dbEntity) where TEntity : DbEntity
        {
            if (dbEntity.UserCreatedID == null)
                dbEntity.UserCreatedID = Client.CurrentUser?.ID;

            var entity = dbEntity as Entity;
            if (entity != null)
                entity.UserModifiedID = Client.CurrentUser?.ID;

            dbEntity.DateCreated = DateTime.Now;

            var datasource = Repository<TEntity>();
            datasource.Add(dbEntity);
            datasource.SaveChanges();
            RefreshContext();
            return dbEntity;
        }

        public void RefreshContext()
        {
            RepositoryFactory.Current.RefreshContext();
        }

        public string ToUrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}