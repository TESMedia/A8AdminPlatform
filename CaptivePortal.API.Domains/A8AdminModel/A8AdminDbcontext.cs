using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.Models.CaptivePortalModel;

namespace CaptivePortal.API.Models.A8AdminModel
{
    public class A8AdminDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public A8AdminDbContext()
            : base("CPAdminDefaultConnection")
        {
        }

        public DbSet<UsersAddress> UsersAddress { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Nas> Nas { get; set; }
        public DbSet<Organisation> Organisation { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<RadGroupCheck> RadGroupCheck { get; set; }
        public DbSet<Radacct> Radacct { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<FormControl> FormControl { get; set; }
        public DbSet<Age> Age { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<MacAddress> MacAddress { get; set; }
        public DbSet<ApiAccessUserSession> UserSession { get; set; }
        public DbSet<AdminSiteAccess> AdminSiteAccess { get; set; }
        public DbSet<ManagePromotion> ManagePromotion { get; set; }
        public DbSet<Group> Group { get; set; }


        public static A8AdminDbContext Create()
        {
            return new A8AdminDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "dbo");
        }
    }
}