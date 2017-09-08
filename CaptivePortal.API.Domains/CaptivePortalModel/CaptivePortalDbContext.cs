//using CaptivePortal.API.Models.CustomIdentity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CaptivePortal.API.Models.CaptivePortalModel
//{
//    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
//    public class CaptivePortalDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
//        ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
//    {
//        public CaptivePortalDbContext() : base("A8PlatFormDefaultConnection")
//        {
//            Database.SetInitializer<CaptivePortalDbContext>(null);
//            Database.CreateIfNotExists();
//        }

//        public DbSet<Form> Form { get; set; }
//        public DbSet<FormControl> FormControl { get; set; }
//        public DbSet<ManagePromotion> ManagePromotion { get; set; }
//        public DbSet<ApiAccessUserSession> UserSession { get; set; }

//    }
//}
