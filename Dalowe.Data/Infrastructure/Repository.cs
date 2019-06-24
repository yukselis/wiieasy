using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dalowe.Domain.Base.Interfaces;

namespace Dalowe.Data.Infrastructure
{
    public class Repository<TEntity> where TEntity : class, IDbEntity
    {
        internal DbSet<TEntity> DbSet;
        internal UnitOfWork UnitOfWork;

        public Repository(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            DbSet = Context.Set<TEntity>();
        }

        internal DbContext Context => UnitOfWork.Context;

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty.Trim());

            if (orderBy != null)
                return orderBy(query).ToList();

            return query.ToList();
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            var resultList = Get(filter, null, includeProperties);
            var result = resultList?.FirstOrDefault();

            return result;
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity == null || !UnitOfWork.OnBeforeInsert(this, entity))
                return;

            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null || !UnitOfWork.OnBeforeDelete(this, entityToDelete))
                return;

            if (Context.Entry(entityToDelete).State == EntityState.Detached)
                DbSet.Attach(entityToDelete);

            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null || !UnitOfWork.OnBeforeUpdate(this, entityToUpdate))
                return;

            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}