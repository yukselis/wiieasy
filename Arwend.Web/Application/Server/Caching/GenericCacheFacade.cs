using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Arwend.Reflection;

namespace Arwend.Web.Application.Server
{
    public abstract class CacheFacade<TEntity> where TEntity : class
    {
        private List<TEntity> oEntities;
        protected static readonly object Access_Locker = new object();
        protected virtual string EntityIdentifier { get { return "Key"; } }
        //protected abstract string EntityIdentifier { get; }
        protected abstract string Key { get; }
        protected abstract List<TEntity> GetData();

        public List<TEntity> List
        {
            get
            {
                if (!ConfigurationManager.CachingEnabled) return this.GetData();
                this.oEntities = (List<TEntity>)CacheManager.Get(this.Key);
                if (this.oEntities == null)
                {
                    lock (Access_Locker)
                    {
                        this.oEntities = (List<TEntity>)CacheManager.Get(this.Key);
                        if (this.oEntities == null)
                        {
                            this.oEntities = this.GetData();
                            CacheManager.Add(this.Key, this.oEntities);
                        }
                    }
                }
                return this.oEntities;
            }
        }

        public TEntity Get(object key)
        {
            if (key != null) {
                return this.Get(this.EntityIdentifier, key);
            }
            return null;
        }

        public TEntity Get(string property, object value) 
        {
            return this.List.Where(property, value).FirstOrDefault();
        }

        public List<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return this.List.Where(predicate).ToList();
        }

        public void DropCache()
        {
            CacheManager.Remove(this.Key);
        }

        public bool Reload()
        {
            this.DropCache();
            return this.Load();
        }

        public bool Load() {
            return this.List.Count > 0;
        }

        public void Remove(object id)
        {
            TEntity entity = this.List.Where(this.EntityIdentifier, id).FirstOrDefault();
            if (entity != null)
            {
                this.List.Remove(entity);
            }
        }

        public void Update(TEntity entity)
        {
            this.Remove(entity.GetPropertyValue(this.EntityIdentifier));
            if(this.CanAdd(entity))
                this.List.Add(entity);
        }

        protected virtual bool CanAdd(TEntity entity) {
            return true;
        }
    }
}
