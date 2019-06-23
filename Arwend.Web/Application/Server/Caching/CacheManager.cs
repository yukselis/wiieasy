using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;

namespace Arwend.Web.Application.Server
{
    public static class CacheManager
    {
        private static MemoryCache _CacheContext = MemoryCache.Default;
        private static object _FileLocker = new object();
        public static int CacheDuration
        {
            get { return ConfigurationManager.GetParameter<Int32>("CacheDuration", 1440); }
        }
        public static long CacheCount { get { return _CacheContext.GetCount(); } }
        public static bool Add(string key, object value, int duration = 0)
        {
            return Add(key, value, DateTime.Now.AddMinutes(duration));
        }
        public static bool Add(string key, object value, DateTime absoluteExpiration)
        {
            bool result = false;
            try
            {
                _CacheContext.Set(key, value, GetCachePolicy(key, absoluteExpiration));
                result = true;
            }
            catch { }
            return result;
        }
        public static bool Add(string keyGroup, string keyItem, object value, int duration = 0)
        {
            return Add(keyGroup, keyItem, value, DateTime.Now.AddMinutes(duration));
        }
        public static bool Add(string keyGroup, string keyItem, object value, DateTime absoluteExpiration)
        {
            bool result = true;
            Dictionary<string, object> list = (Dictionary<string, object>)Get(keyGroup);
            if (list == null)
            {
                list = new Dictionary<string, object>();
                Add(keyGroup, list, absoluteExpiration);
            }
            object objectValue = null;
            if (list.TryGetValue(keyItem, out objectValue) && objectValue != null)
                list[keyItem] = value;
            else
                list.Add(keyItem, value);

            return result;
        }
        public static bool ClearAll()
        {
            bool result = false;
            if (_CacheContext.GetCount() > 0)
            {
                result = true;
                foreach (var cache in _CacheContext)
                {
                    if (!Remove(cache.Key.ToString()))
                        result = false;
                }
            }
            return result;
        }
        public static bool Remove(string key)
        {
            bool result = false;
            try
            {
                string[] keys = key.Split(',');

                if (keys != null && keys.Length > 0)
                {
                    foreach (string sKey in keys)
                    {
                        if (_CacheContext.Contains(key))
                        {
                            _CacheContext.Remove(sKey);
                            result = true;
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static object Get(string key)
        {
            return _CacheContext[key] as Object;
        }
        public static object Get(string keyGroup, string keyItem)
        {
            object objectValue = null;
            Dictionary<string, object> list = (Dictionary<string, object>)Get(keyGroup);
            if (list != null)
                list.TryGetValue(keyItem, out objectValue);

            return objectValue;
        }
        private static CacheItemPolicy GetCachePolicy(string key, DateTime absoluteExpiration)
        {
            if (absoluteExpiration <= DateTime.Now) absoluteExpiration = DateTime.Now.AddMinutes(CacheDuration);
            CacheItemPolicy cachingPolicy = new CacheItemPolicy
              {
                  Priority = CacheItemPriority.Default,
                  AbsoluteExpiration = absoluteExpiration,
                  RemovedCallback = new CacheEntryRemovedCallback(OnCachedItemRemoved)
              };
            try
            {
                //string baseDirectory = string.Empty;

                //if (HttpContext.Current != null)
                //    baseDirectory = HttpContext.Current.Server.MapPath("~");
                //else
                //    baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                //lock ((_FileLocker))
                //{
                //    if (!Directory.Exists(baseDirectory + "\\CachingData"))
                //        Directory.CreateDirectory(baseDirectory + "\\CachingData");
                //}
                //cachingPolicy.ChangeMonitors.Add(_MemoryCacheContext.CreateCacheEntryChangeMonitor(new[] { key }));
                //cachingPolicy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<String> { baseDirectory + "\\CachingData\\cache.txt" }));
            }
            catch (Exception)
            {

            }
            return cachingPolicy;
        }
        private static void OnCachedItemRemoved(CacheEntryRemovedArguments arguments)
        {
            string strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", arguments.CacheItem.Value.ToString());
        }
    }
}
