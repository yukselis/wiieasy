using System;

namespace Arwend.Web.Application.Server.Caching.Interfaces
{
    public interface ICache : IDisposable
    {
        void Set<T>(string key, T o) where T : class;
        void Set<T>(string key, T o, DateTime duration) where T : class;
        void SetType<T>(string key, T o, DateTime duration);
        T Get<T>(string key);
        bool Get<T>(string key, out T o) where T : class;
        bool Exists(string key);
        void Delete(string key);
        void DeleteAll();
    }
}
