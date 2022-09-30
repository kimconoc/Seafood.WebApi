using StoreProduct.Domain.Models.DataAccessModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreProduct.Repository
{
    public class EmergencyDepartmentContext : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'DataAccess.Model1' database on your LocalDb instance.
        //
        // If you wish to target a different database and/or database provider, modify the 'Model1'
        // connection string in the application configuration file.
        public EmergencyDepartmentContext()
            : base("EmergencyDepartmentContext")
        {
            Database.CommandTimeout = 240;
            // Database.Log = s => EmergencyDepartmentContextLogger.Log("EFApp", s);
            // Code tạo DbMigrations, không dùng nên comment lại
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmergencyDepartmentContext, Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<SessionAuthorize> SessionAuthorizes { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
