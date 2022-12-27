namespace _4_EntityFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_4_EntityFramework.Models.AppDbContext>
    {
        public Configuration()
        {
            //ponemos en true por fines didacticos, en la practica no es recomendable poner en true
            //esto permite realizar libremente el update-database
            AutomaticMigrationsEnabled = true;
            //permite la perdida de datos en la base de datos cuando realizamos un update-database
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(_4_EntityFramework.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
