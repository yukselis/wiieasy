using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using Dalowe.Domain.Base.Interfaces;

namespace Dalowe.Data.Infrastructure
{
    public delegate bool EntityTranformationEventHandler(object sender, IDbEntity dbEntity);

    public sealed class UnitOfWork : IDisposable
    {
        internal readonly DbContext Context;

        private bool _disposed;
        private Hashtable _repositories;

        private UnitOfWork(string connectionString, bool autoDetectChangesEnabled)
        {
            Context = new DataContext(connectionString);
            Context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Commit();
                Context.Dispose();
            }

            _disposed = true;
        }

        public event EntityTranformationEventHandler BeforeInsert;
        public event EntityTranformationEventHandler BeforeUpdate;
        public event EntityTranformationEventHandler BeforeDelete;

        public static UnitOfWork GetInstance(string connectionString, bool autoDetectChangesEnabled = true)
        {
            return new UnitOfWork(connectionString, autoDetectChangesEnabled);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        {
            var entries = Context
                .ChangeTracker
                .Entries()
                .ToList();

            foreach (var entry in entries)
                entry.Reload();
        }

        public Repository<TEntity> Repository<TEntity>() where TEntity : class, IDbEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
                return (Repository<TEntity>)_repositories[type];

            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), this);

            _repositories.Add(type, repositoryInstance);

            return (Repository<TEntity>)_repositories[type];
        }

        internal bool OnBeforeInsert(object sender, IDbEntity dbEntity)
        {
            return BeforeInsert?.Invoke(sender, dbEntity) ?? true;
        }

        internal bool OnBeforeUpdate(object sender, IDbEntity dbEntity)
        {
            return BeforeUpdate?.Invoke(sender, dbEntity) ?? true;
        }

        internal bool OnBeforeDelete(object sender, IDbEntity dbEntity)
        {
            return BeforeDelete?.Invoke(sender, dbEntity) ?? true;
        }
    }
}