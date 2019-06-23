using Arwend.Web.Application.Server.Caching.Interfaces;
using System;

namespace Arwend.Web.Application.Server.Caching.Models
{
    public class HttpContextCache : ICache
    {
        public void Set<T>(string key, T o) where T : class
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T o, DateTime duration) where T : class
        {
            throw new NotImplementedException();
        }

        public void SetType<T>(string key, T o, DateTime duration)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Get<T>(string key, out T o) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
