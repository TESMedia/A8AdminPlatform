using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    public class LocationDashBoardDbContext : DbContext
    {
        public LocationDashBoardDbContext() : base("Discovery1")
        {
            //Database.SetInitializer<LocationDashBoardDbContext>(null);
        }

        public LocationDashBoardDbContext(string connectionstringName)
            : base(connectionstringName)
        {
            //Database.SetInitializer<LocationDashBoardDbContext>(null);
        }

        public DbSet<LocationIndicator> LocationIndicator { get; set; }
        public DbSet<InterestLocation> InterestLocation { get; set; }
        public DbSet<DataFile> DataFiles { get; set; }
        public DbSet<CruisedDiscovery> CruisedDiscovery { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<NeighBourArea> NeighBourAreas { get; set; }
        private static String GetConnectionString()
        {
            return "Discovery1";
        }

        public static LocationDashBoardDbContext GetNewContextAsPerName(string connectionstringName)
        {
            return new LocationDashBoardDbContext(connectionstringName);
        }

        public static LocationDashBoardDbContext Create()
        {
            return new LocationDashBoardDbContext(GetConnectionString());
        }

        //migration for changing defualt AspNetUser table to customize one.
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DataFile>().ToTable("DataFiles");
            modelBuilder.Entity<Parameter>().ToTable("Parameters");
            modelBuilder.Entity<CruisedDiscovery>().ToTable("CruisedDiscovery");
            modelBuilder.Entity<NeighBourArea>().ToTable("NeighBourAreas");
        }
    }
}