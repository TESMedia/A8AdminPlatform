using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Models.RTLSModel;

namespace CaptivePortal.API.Models.A8AdminModel
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class A8AdminDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public A8AdminDbContext()
        : base("A8PlatFormDefaultConnection")
        {
            Database.SetInitializer(new MySqlInitializer());
            this.Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<A8AdminDbContext>(null);
            Database.CreateIfNotExists();
        }

      

        //Users Module with Master A8PlatForm Module
        public DbSet<UsersAddress> UsersAddress { get; set; }
        public DbSet<Age> Age { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<AdminSiteAccess> AdminSiteAccess { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<MacAddress> MacAddress { get; set; }


        ////Captive Portal Module with Master A8PlatForm Module

        public DbSet<Form> Form { get; set; }
        public DbSet<FormControl> FormControl { get; set; }
        public DbSet<ManagePromotion> ManagePromotion { get; set; }
        public DbSet<ApiAccessUserSession> UserSession { get; set; }


        //RTLS Module with Master A8PlatForm Module
        public DbSet<Device> Device { get; set; }
        public DbSet<LocationData> LocationData { get; set; }
        public DbSet<TrackMember> CheckMembers { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }
        public DbSet<RtlsConfiguration> RtlsConfigurations { get; set; }

        public static A8AdminDbContext Create()
        {
            return new A8AdminDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "dbo");
           
        }
    }
}