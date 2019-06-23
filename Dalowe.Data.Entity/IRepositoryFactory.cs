namespace Dalowe.Data.Entity
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepositoryFromEntity<TEntity>() where TEntity : Domain.Base.DbEntity;
        TRepository GetRepository<TRepository>();
        void RefreshContext();
    }
}