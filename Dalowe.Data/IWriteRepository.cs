using System;

namespace Dalowe.Data
{
    public interface IWriteRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Create();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Attach(TEntity entity);
        void Update(TEntity entity);
        void SaveChanges(bool ignoreChangeTracker = false);
        void Rollback();
    }
}