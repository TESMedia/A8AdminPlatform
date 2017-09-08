using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using log4net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Data.Entity;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Controllers.CPAdmin
{
    public class AccountController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(AdminController));
        private ApplicationRoleManager _roleManager;
        private A8AdminDbContext db = new A8AdminDbContext();


        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // GET: Global Admin
        public ActionResult Login()
        {
            return View();
        }

        //Authenticate user
        [HttpPost]
        [Route("GAlogin")]
        public async Task<ActionResult> GALogin(AdminLoginViewModel model, string returnUrl)
        {
            try
            {
                ApplicationUser existUser = db.Users.Where(u => u.Email == model.UserName).FirstOrDefault();
                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.PasswordHash, model.RememberMe, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        //return RedirectToAction("Home", "Admin", new { SiteId = existUser.SiteId, UserName = existUser.UserName });
                        //return Json("success", JsonRequestBehavior.AllowGet);
                        ViewBag.roleOfUser = UserManager.GetRoles(existUser.Id).FirstOrDefault();
                        return RedirectToAction("Index", "Home");

                    case SignInStatus.Failure:
                    default:
                        // ModelState.AddModelError("", "Invalid login attempt.");
                        TempData["SuccessReset"] = "Invalid login attempt.";
                        return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

      
        //Get:Admin will create user.
        public ActionResult Register()
        {
            ViewBag.groups = from item in db.Group.ToList()
                             select new SelectListItem()
                             {
                                 Text = item.GroupName,
                                 Value = item.GroupId.ToString(),
                             };
            ViewBag.sites = from item in db.Site.ToList()
                            select new SelectListItem()
                            {
                                Value = item.SiteId.ToString(),
                                Text = item.SiteName
                            };
            return View();
        }

        //Get:Reset password
        public ActionResult ResetPassword(int userId, string code)
        {
            ResetPasswordViewModel objResetPassword = new ResetPasswordViewModel();
            try
            {
                    if (userId != 0)
                    {
                        objResetPassword.Email = UserManager.FindById(userId).Email;
                        var Code = code.Replace(" ", "+");
                        objResetPassword.Code = Code;
                    }
                    return View(objResetPassword);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return View(objResetPassword);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ResetPasswordForNewUser(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError("", "Same EmailId is not exist ");
                return View();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                string roleName = UserManager.GetRoles(user.Id).FirstOrDefault();
                if (roleName == "BusinessUser" && !(String.IsNullOrEmpty(user.Sites.DashboardUrl)))
                {
                    return RedirectPermanent(user.Sites.DashboardUrl);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View();
        }

        //Get: user
        public ActionResult CreateUser(int? siteId)
        {
            SitelistViewModel list = new SitelistViewModel();
            try
            {
                int compId = db.Site.FirstOrDefault(m => m.SiteId == siteId).Company.CompanyId;
                list.SiteViewlist = new List<SiteViewModel>();
                var siteList = db.Site.Where(m => m.CompanyId == compId).ToList();

                var siteViewModelList = (from item in siteList
                                         select new SiteViewModel()
                                         {
                                             SiteName = item.SiteName,
                                             SiteId = item.SiteId
                                         }).ToList();
                list.SiteViewlist.AddRange(siteViewModelList);
                ViewBag.sites = from item in db.Site.Where(m => m.CompanyId == compId).ToList()
                                select new SelectListItem()
                                {
                                    Value = item.SiteId.ToString(),
                                    Text = item.SiteName
                                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(list);

        }

        
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateUserWithRole(CreateUserWithRoleViewModel model, FormCollection fc, string[] RestrictedSites)
        {
            string defaultSiteName = db.Site.FirstOrDefault(m => m.SiteId == model.SiteDdl).SiteName;
            try
            {

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    CreationDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    BirthDate=DateTime.Now,
                    SiteId = model.SiteDdl,
                    Status = Status.Active.ToString(),
                    PhoneNumber = defaultSiteName//Store the SiteName As default Site in Identity Column named PhoneNumber
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.RoleId);
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Welcome to the Captive portal Dashboard", "You are receiving this email as you have been set up as a user of the captive portal Dashboard. To complete the registration process please click <a href=\"" + callbackUrl + "\">here</a>" + " " + "to reset your password and login.If you have any issues with the login process, or were not expecting this email, please email support@airloc8.com.");
                    TempData["Success"] = "An Email has sent to your Inbox.";

                    AdminSiteAccess objAdminSite = new AdminSiteAccess();
                    objAdminSite.UserId = user.Id;
                    objAdminSite.SiteId = model.SiteDdl;
                    db.AdminSiteAccess.Add(objAdminSite);
                    db.SaveChanges();
                }
                else
                {
                    TempData["Success"] = "Username" + " " + model.Email + " " + "already taken.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateUser", "Account", new { SiteId = model.SiteDdl });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDetails(int? siteId, int? userId, int? page, string userName, string foreName, string surName, int? NumberOfLines, int? GroupName)
        {
            WifiUserlistViewModel list = new WifiUserlistViewModel();
            list._menu = db.Group.ToList();
            list.GroupDdl = Convert.ToInt32(GroupName);
            ViewBag.sites = from item in db.Site.ToList()
                            select new SelectListItem()
                            {
                                Value = item.SiteId.ToString(),
                                Text = item.SiteName
                            };

            if (siteId == null)
            {
                siteId = 1;
                ViewBag.SiteName = db.Site.FirstOrDefault(m => m.SiteId == siteId).SiteName;

            }

            //userId = User.Identity.GetUserId();
            list.WifiUserViewlist = new List<WifiUserViewModel>();
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int PageSize = Convert.ToInt32(NumberOfLines);

            var userList = db.Users.Where(m => m.SiteId == siteId).ToList();

            if (userList.Count != 0)
            {
                if (GroupName != null)
                {
                    int roleId = 4;
                    userList = db.Users
            .Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId))
            .ToList();
                }
                else
                {
                    int roleId = 4;
                    userList = db.Users
            .Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId))
            .ToList();
                }

                if (NumberOfLines != null)
                {
                    PageSize = Convert.ToInt32(NumberOfLines);
                    ViewBag.selectedNumber = NumberOfLines;
                }
                else
                {
                    PageSize = 20;
                }

                var TotalPages = (int)Math.Ceiling((decimal)userList.Count / (decimal)PageSize);

                var startPage = currentPageIndex - 5;
                int endPage = currentPageIndex + 4;
                if (startPage <= 0)
                {
                    endPage -= (startPage - 1);
                    startPage = 1;
                }
                if (endPage > TotalPages)
                {
                    endPage = TotalPages;
                    if (endPage > 10)
                    {
                        startPage = endPage - 9;
                    }
                }
                //Search user according to Group
                if (GroupName != 0 & GroupName != null)
                {
                    userList = db.Users.Where(m => m.GroupId == GroupName).ToList();
                }
                //var userList = db.Users.Where(m => m.SiteId == siteId).ToList();
                //If Searching on the basis of the single parameter
                if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(foreName) || !string.IsNullOrEmpty(surName))
                {
                    //if (!string.IsNullOrEmpty(foreName))
                    //{
                    //    //For the parameter contain only foreName  for searching or filter
                    //    if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(surName))
                    //    {
                    //        userList = db.Users.Where(p => p.FirstName.ToLower().Contains(foreName.ToLower())).ToList().Skip(((int)currentPageIndex - 1) * PageSize).Take(PageSize).ToList();
                    //        TotalPages = (int)Math.Ceiling((double)db.Users.Where(p => p.FirstName.ToLower() == foreName.ToLower()).Count() / PageSize);
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(surName))
                    //{
                    //    //For the parameter contain only surName  for searching or filter
                    //    if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(foreName))
                    //    {
                    //        userList = db.Users.Where(p => p.LastName.ToLower().Contains(surName.ToLower())).ToList().Skip(((int)currentPageIndex - 1) * PageSize).Take(PageSize).ToList();
                    //        TotalPages = (int)Math.Ceiling((double)db.Users.Where(p => p.LastName.ToLower() == surName.ToLower()).Count() / PageSize);
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(userName))
                    //{
                    //    //For the parameter contain only username  for searching or filter
                    //    if (string.IsNullOrEmpty(foreName) && string.IsNullOrEmpty(surName))
                    //    {
                    //        userList = db.Users.Where(p => p.UserName.ToLower().Contains(userName.ToLower())).ToList().Skip(((int)currentPageIndex - 1) * PageSize).Take(PageSize).ToList();
                    //        TotalPages = (int)Math.Ceiling((double)db.Users.Where(p => p.UserName.ToLower() == userName.ToLower()).Count() / PageSize);
                    //    }
                    //}
                }
                //If the Searching contain no parameter
                //else
                //{
                userList = userList.Skip(((int)currentPageIndex - 1) * PageSize).Take(PageSize).ToList();
                //TotalPages = (int)Math.Ceiling((decimal)db.Users.Count() / PageSize);
                //}
                //if (userList.Count != 1)
                //{
                var userViewModelList = (from item in userList
                                         select new WifiUserViewModel()
                                         {
                                             SiteId = Convert.ToInt32(item.SiteId),
                                             UserId = item.Id,
                                             UserName = item.UserName,
                                             FirstName = item.FirstName,
                                             LastName = item.LastName,
                                             CreationDate = item.CreationDate,
                                             Lastlogin = item.UpdateDate,
                                             //SiteName= SiteName
                                             // Password = item.Password,
                                             MacAddress = db.MacAddress.Where(x => x.UserId == item.Id).OrderByDescending(x => x.MacId).Take(1).Select(x => x.MacAddressValue).ToList().FirstOrDefault()

                                         }).ToList();
                list.WifiUserViewlist.AddRange(userViewModelList);
                //}
                //else
                //{
                //    TempData["userSuc"] = "No data found";
                //}

                if (userId != null)
                {
                    list.WifiUserView = userViewModelList.FirstOrDefault(m => m.UserId == userId);
                }
                else
                {
                    list.WifiUserView = userViewModelList.FirstOrDefault();
                    list.WifiUserView._menu = db.Group.ToList();
                    list.WifiUserView.GroupDdl = Convert.ToInt32(GroupName);
                }
                ViewBag.CurrentPage = currentPageIndex;
                ViewBag.PageSize = PageSize;
                ViewBag.TotalPages = TotalPages;
                ViewBag.foreName = foreName;
                ViewBag.surName = surName;
                ViewBag.userName = userName;
            }
            else
            {

            }

            return View(list);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserWithProfile(int SiteId, int userId)
        {
           
           // var userid = User.Identity.GetUserId();
            var userDetail = db.Users.FirstOrDefault(m => m.Id== userId);
            var termConditionVersion = db.Site.FirstOrDefault(m => m.SiteId == SiteId).Term_conditions;
            var siteName = db.Site.FirstOrDefault(m => m.SiteId == SiteId).SiteName;
            var model = new MacAddressViewModel();
            WifiUserViewModel objUserViewModel = new WifiUserViewModel();
            objUserViewModel._menu = db.Group.ToList();
            //objUserViewModel._menu = db.Group.ToList();
            
            
            if (userDetail != null)
            {
                objUserViewModel.Password = userDetail.PasswordHash;
                objUserViewModel.UserName = userDetail.UserName;
                objUserViewModel.Gender = db.Gender.FirstOrDefault(m => m.GenderId == userDetail.GenderId) == null ? null : db.Gender.FirstOrDefault(m => m.GenderId == userDetail.GenderId).Value;
                objUserViewModel.AgeRange = db.Age.FirstOrDefault(m => m.AgeId == userDetail.AgeId) == null ? null : db.Age.FirstOrDefault(m => m.AgeId == userDetail.AgeId).Value;
                objUserViewModel.AutoLogin = Convert.ToBoolean(userDetail.AutoLogin);
                objUserViewModel.Term_conditions = termConditionVersion;
                objUserViewModel.PromotionEmailOptIn = Convert.ToBoolean(userDetail.promotional_email);
                objUserViewModel.ThirdPartyOptIn = Convert.ToBoolean(userDetail.ThirdPartyOptIn);
                objUserViewModel.UserOfDataOptIn = Convert.ToBoolean(userDetail.UserOfDataOptIn);
                //objUserViewModel.Status = (Status)Enum.Parse(typeof(Status), userDetail.Status);
                var mac = db.MacAddress.Where(m => m.Users.SiteId == SiteId).ToList();
                //var lastEntry = db.MacAddress.LastOrDefault(m => m.UserId == UserId).MacAddressValue;
                //objUserViewModel.MacAddress = lastEntry;
                objUserViewModel.MacAddressList = mac;
            }
            return PartialView("_UserDetails", objUserViewModel);
        }

        public ActionResult ManageUser(int? siteId, int? page, string userName, int? NumberOfLines)
        {
            UserlistViewModel list = new UserlistViewModel();
            var userList = db.Users.Where(u => !u.Roles.Any(r => r.RoleId == 4)).ToList();
            
            int PageSize = Convert.ToInt32(NumberOfLines);
            if (NumberOfLines != null)
            {
                PageSize = Convert.ToInt32(NumberOfLines);
                ViewBag.selectedNumber = NumberOfLines;
            }
            else
            {
                PageSize = 20;
            }

            var TotalPages = (int)Math.Ceiling((decimal)userList.Count / (decimal)PageSize);
            try
            {
                if (siteId != null)
                {
                    var userId = User.Identity.GetUserId();
                    list.UserViewlist = new List<UserViewModel>();
                    int currentPageIndex = page.HasValue ? page.Value : 1;
                    var startPage = currentPageIndex - 5;
                    int endPage = currentPageIndex + 4;
                    if (startPage <= 0)
                    {
                        endPage -= (startPage - 1);
                        startPage = 1;
                    }
                    if (endPage > TotalPages)
                    {
                        endPage = TotalPages;
                        if (endPage > 10)
                        {
                            startPage = endPage - 9;
                        }
                    }

                    userList = userList.Skip(((int)currentPageIndex - 1) * PageSize).Take(PageSize).ToList();
                    // TotalPages = (int)Math.Ceiling((decimal)userList.Count / PageSize);

                    var userViewModelList = (from item in userList
                                             select new UserViewModel()
                                             {
                                                 SiteId = siteId.Value,
                                                 UserId = item.Id.ToString(),
                                                 UserName = item.UserName,
                                                 CreationDate = item.CreationDate,
                                                 Lastlogin = item.UpdateDate,
                                                 //Status = item.Status
                                                 Role = UserManager.GetRoles(item.Id).FirstOrDefault()


                                             }).ToList();
                    list.UserViewlist.AddRange(userViewModelList);

                    //if (userId != null)
                    //{
                    //    list.UserView = userViewModelList.FirstOrDefault(m => m.UserId == userId);
                    //}
                    //else
                    //{
                    //list.UserView = userViewModelList.FirstOrDefault();
                    //}
                    ViewBag.CurrentPage = currentPageIndex;
                    ViewBag.PageSize = PageSize;
                    ViewBag.TotalPages = TotalPages;
                    ViewBag.userName = userName;
                }
                else
                {
                    TempData["SiteIdCheck"] = "Please select any of the site and then manage user or If site is not there create new site";
                    return RedirectToAction("Index", "Home");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(list);


        }

        [HttpPost]
        public ActionResult UpdateUser(FormCollection fc)
        {
            var UserNameBefore = fc["UserName"];
            int userId = Convert.ToInt16(fc["UserId"]);

            try
            {
                //Updating the Users table
                if (db.Users.Any(m => m.UserName == UserNameBefore))
                {
                    //userId = db.Users.Where(m => m.UserName == UserNameBefore).FirstOrDefault().UserId;
                    var objUser = db.Users.Find(userId);
                    {
                        //objUser.UserName = fc["UserName"];
                        objUser.GenderId = Convert.ToInt32(fc["GenderId"]);
                        objUser.AgeId = Convert.ToInt32(fc["AgeId"]);

                        objUser.MobileNumer = Convert.ToInt32(fc["MobileNumber"]);
                        objUser.Status = Convert.ToString(fc["Status"]);
                        objUser.Status = fc["Status"].ToString();

                       // objUser.Email = fc["UserName"];
                        db.Entry(objUser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("UserDetails", "Account");
        }

        [HttpPost]
        public void DeleteUser(int UserId)
        {
            ApplicationUser user = db.Users.Find(UserId);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public ActionResult UpdatePassword(int UserId)
        {
            return View();
        }


        [HttpPost]
        public ActionResult UpdatePassword(FormCollection fc)
        {
            int userId = Convert.ToInt16(fc["UserId"]);
            if (fc["NewPassword"] == fc["ConfirmPassword"])
            {
                var objUser = db.Users.Find(userId);
                {
                    objUser.PasswordHash = fc["NewPassword"];
                    db.Entry(objUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("UserDetails", "Account");
        }

    }
}