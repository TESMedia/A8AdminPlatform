//using System.Data.Entity;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using CaptivePortal.API.Models.RTLSModel;

//namespace CaptivePortal.API.Models.RTLSModel
//{
//    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
//    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
//    public class RTLSDbContext : DbContext
//    {
//        public RTLSDbContext() : base("A8PlatFormDefaultConnection")
//        {
//            Database.SetInitializer<RTLSDbContext>(null);
//            Database.CreateIfNotExists();
//        }
//        public DbSet<Device> Device { get; set; }
//        public DbSet<LocationData> LocationData { get; set; }
//        public DbSet<TrackMember> CheckMembers { get; set; }
//        public DbSet<AppLog> AppLogs { get; set; }
//        public DbSet<RtlsConfiguration> ConfigurationParameters { get; set; }

//        public static RTLSDbContext Create()
//        {
//            return new RTLSDbContext();
//        }
//    }
//}