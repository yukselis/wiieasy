using System;
using Arwend.Web.Application.Server.Caching.Helpers;
using Arwend.Web.Application.Server.Caching.Interfaces;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Arwend.Web.Application.Server.Caching.Models
{
    public sealed class Memcached : ICache
    {
        private static readonly object SyncObject = new object();

        private static volatile Memcached _instance;
        private readonly MemcachedClient _memcachedClient;

        public static Memcached Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new Memcached();
                        }
                    }
                }
                return _instance;
            }
        }

        private Memcached()
        {
            _memcachedClient = new MemcachedClient();
        }

        public void Set<T>(string key, T o) where T : class
        {
            _memcachedClient.Store(StoreMode.Set, key, o, MemcachedClient.Infinite);
        }

        public void Set<T>(string key, T o, DateTime duration) where T : class
        {
            _memcachedClient.Store(StoreMode.Set, key, o, duration);
        }

        public void SetType<T>(string key, T o, DateTime duration)
        {
            _memcachedClient.Store(StoreMode.Set, key, o, duration);
        }

        public T Get<T>(string key)
        {
            return _memcachedClient.Get<T>(key);
        }

        public bool Exists(string key)
        {
            return _memcachedClient.Get(key) != null;
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

        public void Delete(string key)
        {
            try
            {
                _memcachedClient.Remove(key);
            }
            catch
            {
                //LogHelper.Log(new LogModel { EventType = Global.Enums.LogType.Error, Exception = ex, Message = "Memcached", MessageDetail = "Delete" });
            }
        }

        public void DeleteAll()
        {
            _memcachedClient.FlushAll();
        }

        public void Dispose()
        {
            _memcachedClient.Dispose();
        }
    }
}
