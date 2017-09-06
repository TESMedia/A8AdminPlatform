using System;
using System.Linq;
using Microsoft.AspNet.Identity.Owin;
using CaptivePortal.API.Models.A8AdminModel;
using log4net;
using CaptivePortal.API.Models.CustomIdentity;
using System.Web;
using CaptivePortal.API.ViewModels.CPAdmin;
using Microsoft.AspNet.Identity;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class UserRepository
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private A8AdminDbContext db = null;
        ILog log = LogManager.GetLogger(typeof(UserRepository));


        public UserRepository()
        {
            db = new A8AdminDbContext();
        }
        public UserRepository(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objser"></param>
        public void CreateWifiUser(ApplicationUser objUser)
        {
            try
            {
                //CustomPasswordHasher objPassword = new CustomPasswordHasher();
                //objUser.PasswordHash = objPassword.HashPassword(objUser.PasswordHash);
                //db.Users.Add(objUser);
                //db.SaveChanges();
                UserManager.CreateAsync(objUser);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public void UpdateWifiUser(ApplicationUser objUser)
        {
            try
            {
                db.Entry(objUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        public void CreateAuthenticatedUser(ApplicationUser objUser)
        {

            try
            {
                CustomPasswordHasher objPassword = new CustomPasswordHasher();
                //Save the Users data into Users table
                objUser.CreationDate = DateTime.Now;
                objUser.UpdateDate = DateTime.Now;
                var result = UserManager.CreateAsync(objUser, objUser.PasswordHash);
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        public void CreateUserRole(ApplicationUser objUser)
        {
            try
            {
                UserManager.AddToRoleAsync(objUser.Id, "WiFiUser");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserUniqueId"></param>
        /// <param name="SiteId"></param>
        public void RemoveMemberAsUserUniqueId(string UserUniqueId,int SiteId)
        {
            try
            {
                var objUser = db.Users.FirstOrDefault(m => m.UniqueUserId == UserUniqueId && m.SiteId == SiteId);
                db.Users.Remove(objUser);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public int GetUserId(string UserName)
        {
           return db.Users.FirstOrDefault(m => m.UserName == UserName).Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ApplicationUser MapToApplicationUser(MemberViewModel model)
        {
            ApplicationUser objUser = new ApplicationUser();
            if (db.Users.Any(m => m.UniqueUserId == model.UserId))
            {
                objUser = db.Users.FirstOrDefault(m => m.UniqueUserId == model.UserId);
            }
            objUser.UserName = model.UserName;
            objUser.Email = model.Email;
            objUser.PasswordHash = model.Password;
            objUser.SiteId = model.SiteId;
            objUser.FirstName = model.FirstName;
            objUser.LastName = model.LastName;
            objUser.UniqueUserId = model.UserId;
            objUser.AutoLogin = model.AutoLogin;
            objUser.MobileNumer = model.MobileNumber;
            objUser.GenderId = model.GenderId;
            objUser.AgeId = model.AgeId;
            objUser.BirthDate = model.BirthDate;
            return objUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public ApplicationUser GetUniqueUserPerSite(string UserName, int SiteId)
        {
            return db.Users.FirstOrDefault(m => m.UserName == UserName && m.SiteId == SiteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MacAddress"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public ApplicationUser GetUserPerDeviceMacAddress(string MacAddress, int SiteId)
        {
            return db.MacAddress.FirstOrDefault(m => m.MacAddressValue == MacAddress && m.Users.SiteId == SiteId).Users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsMemeber(string UserName)
        {
            var ObjUser = db.Users.FirstOrDefault(m => m.UserName == UserName);
            if (!string.IsNullOrEmpty(ObjUser.UniqueUserId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool IsNewUserAsPerSite(string UserName, int SiteId)
        {
            return !db.Users.Any(m => m.UserName == UserName && m.SiteId == SiteId);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="UserUniqueId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool IsNewMemberAsPerSite(string UserUniqueId, int SiteId)
        {
            return !db.Users.Any(m => m.UniqueUserId == UserUniqueId && m.SiteId == SiteId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool IsMemberApplicationAdmin(string UserName,string Password)
        {
            return UserManager.FindByName(UserName).PasswordHash == UserManager.PasswordHasher.HashPassword(Password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserUniqueId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool IsMemberRegisterInRTLS(string UserUniqueId, int SiteId)
        {
            return db.MacAddress.FirstOrDefault(m => m.Users.UniqueUserId == UserUniqueId && m.Users.SiteId == SiteId).IsRegisterInRtls;
        }


    }
}