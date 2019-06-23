using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Dalowe.Data.Entity
{
    public class Repository<TEntity> : MarshalByRefObject, IRepository<TEntity> where TEntity : class
    {
        private readonly IDbSet<TEntity> _objectSet;
        protected DbContext Context;

        public Repository(IUnitOfWork unitOfWork)
        {
            Context = unitOfWork as DbContext;
            if (Context != null)
                _objectSet = Context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includes)
        {
            var objectSet = GetQuery(); //.Where(entity => entity.StatusID != 2);
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            if (orderBy != null) return orderBy(objectSet).AsEnumerable();
            return objectSet.AsEnumerable();
        }

        public IEnumerable<TColumn> GetBy<TColumn>(Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TColumn>> columns,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includes)
        {
            var objectSet = GetQuery().Where(exp); //.Where(entity => entity.StatusID != 2)
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            if (orderBy != null) objectSet = orderBy(objectSet);
            return objectSet.Select(columns);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.AsNoTracking().Count(predicate); //.Where(entity => entity.StatusID != 2)
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includes)
        {
            var objectSet = GetQuery().Where(predicate); //.Where(entity => entity.StatusID != 2)
            foreach (var included in includes)
                objectSet = objectSet.Include(included);

            if (orderBy != null) return orderBy(objectSet);
            return objectSet;
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var objectSet = GetQuery(); //.Where(entity => entity.StatusID != 2);
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            return objectSet.SingleOrDefault(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var objectSet = GetQuery(); //.Where(entity => entity.StatusID != 2);
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            return objectSet.FirstOrDefault(predicate);
        }

        public TEntity Last(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var objectSet = GetQuery(); //.Where(entity => entity.StatusID != 2);
            foreach (var included in includes)
                objectSet = objectSet.Include(included);
            return objectSet.LastOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Any(predicate); //.Where(entity => entity.StatusID != 2)
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TEntity Create()
        {
            return Context.Set<TEntity>().Create();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _objectSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            _objectSet.Add(entity);
        }

        public void Attach(TEntity entity)
        {
            _objectSet.Attach(entity);
        }

        public void Update(TEntity entity)
        {
            _objectSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void SaveChanges(bool ignoreChangeTracker = false)
        {
            var validationResultList = Context.GetValidationErrors();
            if (validationResultList.Count() > 0)
                foreach (var validationResult in validationResultList)
                    foreach (var validationError in validationResult.ValidationErrors)
                        throw new Exception(string.Format("PropertyName : {0}, ErrorMessage : {1}",
                            validationError.PropertyName, validationError.ErrorMessage));

            var changeSet = Context.ChangeTracker.Entries();
            if (changeSet != null)
                foreach (var entry in changeSet.Where(c =>
                    c.State != EntityState.Unchanged && c.State != EntityState.Deleted))
                {
                    if (entry.State == EntityState.Added && entry.CurrentValues.PropertyNames.Contains("DateCreated"))
                        entry.CurrentValues["DateCreated"] = DateTime.Now;

                    if (entry.CurrentValues.PropertyNames.Contains("DateModified") && !ignoreChangeTracker)
                        entry.CurrentValues["DateModified"] = DateTime.Now;

                    if (entry.CurrentValues.PropertyNames.Contains("RowGuid") &&
                        Guid.Empty.Equals(entry.CurrentValues["RowGuid"]))
                        entry.CurrentValues["RowGuid"] = Guid.NewGuid();
                }

            Context.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
                if (entry.State != EntityState.Unchanged)
                    entry.State = EntityState.Unchanged;
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _objectSet;
        }

        public void Delete(TEntity entity, long userDeletedId)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            _objectSet.Remove(entity);
            //entity.DateModified = DateTime.Now;
            //entity.StatusID = 2;
            //entity.UserModifiedID = userDeletedId;
        }

        public bool EnableLazyLoading()
        {
            //return Context.EnableLazyLoad();
            return false;
        }

        public bool DisableLazyLoading()
        {
            Context.Configuration.LazyLoadingEnabled = false;
            return !Context.Configuration.LazyLoadingEnabled;
        }

        private ObjectParameter CreateParameter(string name, string value)
        {
            return value != null ? new ObjectParameter(name, value) : new ObjectParameter(name, typeof(string));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (Context != null)
                {
                    Context.Dispose();
                    Context = null;
                }
        }
    }
}