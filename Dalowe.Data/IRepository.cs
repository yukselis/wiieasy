using System;
using System.Linq;

namespace Dalowe.Data
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>, IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> GetQuery();
        bool EnableLazyLoading();
        bool DisableLazyLoading();
    }
}