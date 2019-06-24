using System.Data.Entity;
using Dalowe.Domain.Log;
using Dalowe.Domain.Visa;

namespace Dalowe.Data.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(string nameOrConnectionString = null)
            : base( nameOrConnectionString ?? "name=DataContext")
        {
            //TODO: Her zaman drop etmemeli daha sonradan düzenlenecek.
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

        public virtual DbSet<ActionLog> ActionLogs { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here..
        }
    }
}