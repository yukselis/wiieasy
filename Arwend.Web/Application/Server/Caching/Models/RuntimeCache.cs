using System;
using System.Runtime.Caching;
using Arwend.Web.Application.Server.Caching.Interfaces;

namespace Arwend.Web.Application.Server.Caching.Models
{
    public sealed class RuntimeCache : ICacheRuntime
    {
        private static readonly object SyncObject = new object();
        private static volatile RuntimeCache _instance;
        private ObjectCache _cache;

        public static RuntimeCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new RuntimeCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private RuntimeCache()
        {
            _cache = MemoryCache.Default;
        }

        public void Set<T>(string key, T o) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (o == null)
                return;

            _cache.Set(key, o, DateTime.MaxValue);
        }

        public void Set<T>(string key, T o, DateTime duration) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (o == null)
                return;

            _cache.Set(key, o, duration);
        }

        public void SetType<T>(string key, T o, DateTime duration)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (o == null)
                return;

            _cache.Set(key, o, duration);
        }

        public void SetWithCallback<T>(string key, T o, DateTime duration, CacheEntryRemovedCallback removedCallback, CacheEntryUpdateCallback updateCallback) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (o == null)
                return;

            var policy = new CacheItemPolicy();
            policy.RemovedCallback = removedCallback;
            policy.UpdateCallback = updateCallback;
            policy.AbsoluteExpiration = duration;

            _cache.Set(key, o, policy);
        }

        public void SetTypeWithCallback<T>(string key, T o, DateTime duration, CacheEntryRemovedCallback removedCallback, CacheEntryUpdateCallback updateCallback)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (o == null)
                return;

            var policy = new CacheItemPolicy();
            policy.RemovedCallback = removedCallback;
            policy.UpdateCallback = updateCallback;
            policy.AbsoluteExpiration = duration;

            _cache.Set(key, o, policy);
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            return (T)_cache.Get(key);
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
                o = (T)_cache[key];
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
            return _cache[key] != null;
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        public void DeleteAll()
        {
            MemoryCache.Default.Dispose();
        }

        public void Dispose()
        {
            _cache = null;
            MemoryCache.Default.Dispose();
        }
    }
}
