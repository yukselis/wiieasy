using System;
using System.Linq;
using System.Reflection;

namespace Dalowe.Data.Entity
{
    public class RepositoryFactory : IRepositoryFactory
    {
        [ThreadStatic] private static IRepositoryFactory _current;

        private IUnitOfWork _context;

        private RepositoryFactory()
        {
            if (_current == null)
                _context = new DataContext();
        }

        public static IRepositoryFactory Current
        {
            get
            {
                _current = _current ?? new RepositoryFactory();
                return _current;
            }
        }

        public IRepository<TEntity> GetRepositoryFromEntity<TEntity>() where TEntity : Domain.Base.DbEntity
        {
            return GetInstance<IRepository<TEntity>>();
        }

        public TRepository GetRepository<TRepository>()
        {
            return GetInstance<TRepository>();
        }

        public void RefreshContext()
        {
            _context = new DataContext();
            _current = null;
        }

        private TInstance GetInstance<TInstance>()
        {
            return (TInstance)GetInstance(typeof(TInstance));
        }

        private object GetInstance(Type type)
        {
            type = GetRealType(type);
            var constructor = type.GetConstructors().Single();
            var parameters = constructor.GetParameters();

            var result = parameters.Any()
                ? Activator.CreateInstance(type, parameters.Select(parameter => parameter.ParameterType == typeof(IUnitOfWork) ? _context : GetInstance(parameter.ParameterType)).ToArray())
                : Activator.CreateInstance(type);

            return result;
        }

        private ConstructorInfo GetDefaultConstructor<TInstance>()
        {
            var defaultConstructor = typeof(TInstance).GetConstructor(Type.EmptyTypes);
            return defaultConstructor;
        }

        private Type GetRealType(Type type)
        {
            return typeof(RepositoryFactory).Assembly
                .GetExportedTypes()
                .Where(type.IsAssignableFrom)
                .First(t => !t.IsAbstract && !t.IsInterface);
        }

        public static void ReleaseFactory()
        {
            _current = null;
        }
    }
}