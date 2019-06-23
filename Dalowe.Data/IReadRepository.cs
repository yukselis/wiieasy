using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dalowe.Data
{
    public interface IReadRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includes);

        IEnumerable<TColumn> GetBy<TColumn>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TColumn>> columns, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includes);

        int Count(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includes);

        TEntity Single(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        TEntity First(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        TEntity Last(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        bool Any(Expression<Func<TEntity, bool>> predicate);
    }
}