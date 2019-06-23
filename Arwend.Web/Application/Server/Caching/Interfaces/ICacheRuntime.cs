using System;
using System.Runtime.Caching;

namespace Arwend.Web.Application.Server.Caching.Interfaces
{
    public interface ICacheRuntime : ICache
    {
        /// <summary>
        /// Date-Time specific cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="duration"></param>
        /// <param name="removedCallback">Sadece RuntimeCache de kullanılır</param>
        /// <param name="updateCallback">Sadece RuntimeCache de kullanılır</param>
        void SetWithCallback<T>(string key, T o, DateTime duration, CacheEntryRemovedCallback removedCallback, CacheEntryUpdateCallback updateCallback) where T : class;

        /// <summary>
        /// Reference type olmayan dataları cache'e atmak için kullanılır
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <param name="duration"></param>
        /// <param name="removedCallback">Sadece RuntimeCache de kullanılır</param>
        /// <param name="updateCallback">Sadece RuntimeCache de kullanılır</param>
        void SetTypeWithCallback<T>(string key, T o, DateTime duration, CacheEntryRemovedCallback removedCallback, CacheEntryUpdateCallback updateCallback);
    }

}
