using System;
using System.Collections.Generic;
using System.Linq;
using Arwend.Web.Application.Server.Caching.Helpers;
using Arwend.Web.Application.Server.Caching.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Arwend.Web.Application.Server.Caching.Models
{
    public sealed class RedisCache : ICache
    {
        private static readonly object AccessLocker = new object();

        private static volatile RedisCache _Current;
        private readonly IDatabase _redisDb;
        private readonly ConnectionMultiplexer _redis;

        public static RedisCache Current
        {
            get
            {
                if (_Current == null)
                {
                    lock (AccessLocker)
                    {
                        if (_Current == null)
                            _Current = new RedisCache();
                    }
                }
                return _Current;
            }
        }

        private RedisCache()
        {
            _redis = RedisHelper.Current.Redis;
            if (_redis != null)
                _redisDb = _redis.GetDatabase();
        }

        public List<string> GetKeys(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return null;

            List<string> result = null;
            try
            {
                var server = _redis.GetServer(ConfigurationManager.GetParameter("RedisForCache"));
                var pattern = prefix + "*";
                var keys = server.Keys(pattern: pattern);

                if (keys == null || keys.Count() == 0)
                    return null;

                result = new List<string>();
                foreach (var key in keys)
                {
                    result.Add(key);
                }
            }
            catch
            {
                
            }

            return result;
        }

        public void Set<T>(string key, T o) where T : class
        {
            try
            {
                _redisDb.StringSet(key, JsonConvert.SerializeObject(o));
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.Set<T>", MessageDetail = "Set<T>(string key, T o)", Exception = ex });
            }

        }

        public void Set<T>(string key, T o, DateTime duration) where T : class
        {
            try
            {
                _redisDb.StringSet(key, JsonConvert.SerializeObject(o), duration.ToUniversalTime().Subtract(DateTime.UtcNow));
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.Set<T>", MessageDetail = "Set<T>(string key, T o, DateTime duration)", Exception = ex });
            }

        }

        public void SetType<T>(string key, T o, DateTime duration)
        {
            try
            {
                _redisDb.StringSet(key, JsonConvert.SerializeObject(o), duration.ToUniversalTime().Subtract(DateTime.UtcNow));
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.SetType<T>", MessageDetail = "SetType<T>(string key, T o, DateTime duration)", Exception = ex });
            }

        }

        public T Get<T>(string key)
        {
            var model = default(T);

            try
            {
                model = JsonConvert.DeserializeObject<T>(_redisDb.StringGet(key));
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.Get<T>", MessageDetail = "Get<T>(string key):" + key, Exception = ex });
            }

            return model;

        }

        public bool Get<T>(string key, out T o) where T : class
        {
            try
            {
                if (!Exists(key))
                {
                    o = default(T);
                    return false;
                }
                o = Get<T>(key);
            }
            catch
            {
                o = default(T);
                return false;
            }
            return true;
        }

        public bool Exists(string key)
        {
            try
            {
                return _redisDb.KeyExists(key);
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.Exists", Exception = ex });
            }

            return false;
        }

        public void Delete(string key)
        {
            try
            {
                if (Exists(key))
                    _redisDb.KeyDelete(key);
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Cache, Message = "Redis.Delete", Exception = ex });
            }

        }

        public void DeleteAll()
        {

        }

        public void Dispose()
        {
            if (_redis == null)
                return;

            _redis.Dispose();
        }
    }
}
