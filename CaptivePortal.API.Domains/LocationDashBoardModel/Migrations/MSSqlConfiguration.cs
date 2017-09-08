namespace CaptivePortal.API.Models.LocationDashBoardModel.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    [DbConfigurationType(typeof(MSSqlConfiguration))]
    internal sealed class MSSqlConfiguration : DbMigrationsConfiguration<CaptivePortal.API.Models.LocationDashBoardModel.LocationDashBoardDbContext>
    {
        public MSSqlConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"LocationDashBoardModel\Migrations";
        }

        protected override void Seed(CaptivePortal.API.Models.LocationDashBoardModel.LocationDashBoardDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
