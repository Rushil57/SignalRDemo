using System.Data.Entity;
using Entities.Models;

namespace Entities
{
    public class DBEntities : DbContext
    {
        public DBEntities()
            : base("dbConnectionString")
        {
            Database.SetInitializer<DBEntities>(new CreateDatabaseIfNotExists<DBEntities>());
        }
        //public DbSet<Employee> Employees { get; set; }
        public DbSet<DevTest> DevTest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DBEntities>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
