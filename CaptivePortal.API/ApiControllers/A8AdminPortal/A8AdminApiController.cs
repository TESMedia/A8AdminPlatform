using CaptivePortal.API.Enums.CPEnums;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.Repository.CaptivePortal;
using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace CaptivePortal.API.ApiControllers.A8AdminPortal
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class A8AdminApiController : ApiController
    {
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(A8AdminApiController));
        private ApplicationUserManager _userManager;
        private A8AdminDbContext db = new A8AdminDbContext();
        private RegisterDB objRegisterDB = new RegisterDB();
        StatusReturn objReturn = new StatusReturn();
        AutoLoginStatus returnStatus = new AutoLoginStatus();
        private string retStr = "";
        private string retType = "";
        private int retVal = 0;
        string debugStatus = ConfigurationManager.AppSettings["DebugStatus"];

        public A8AdminApiController()
        {

        }
        public A8AdminApiController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
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

        [HttpPost]
        [Route("a8Captiveportal/V1/RegisterWifiUser")]
        public HttpResponseMessage CreateUserExpose(CreateUserViewModel objUser)
        {
            //log.Info("Enter in a8Captiveportal/V1/CreateUser");
            string logInfoCreateUser = "Enter in a8Captiveportal/V1/CreateUser";
            string logInfoForCreateUserSuccess = null;
           
            string logInfoIsSessionId = null;
            //using (var dbContextTransaction = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            //{
            try
            {
                //First check the Manadatory validation and show the Error Messages
                if (string.IsNullOrEmpty(objUser.UserName))
                {
                    retStr = "Username missing";
                    retVal = Convert.ToInt32(ErrorCodeWarning.UserNameRequired);
                }
                else if (string.IsNullOrEmpty(objUser.Password))
                {
                    retStr = "Password missing";
                    retVal = Convert.ToInt32(ErrorCodeWarning.PasswordRequired);
                }
                else if (objUser.SiteId == 0)
                {
                    retStr = "SiteId missing";
                    retVal = Convert.ToInt32(ErrorCodeWarning.SiteIDRequired);
                }
                //else if (!(db.Site.Any(m => m.SiteId == objUser.SiteId)))
                //{
                //    retStr = "SiteId Not Exist";
                //    retVal = Convert.ToInt32(ErrorCodeWarning.SiteIdNotExist);
                //}


                //if No validation Error then Insert the data into the table
                if (string.IsNullOrEmpty(retStr))
                {
                    //log.Info("if No validation Error then Insert the data into the table");
                    logInfoForCreateUserSuccess = "if No validation Error then Insert the data into the table";
                   var objSite = db.Site.FirstOrDefault(m => m.SiteId == objUser.SiteId);
                    //if Same User with Site Exist then don't allow
                    if (!(db.Users.Any(m => m.UserName == objUser.UserName && m.SiteId == objUser.SiteId)))
                    {

                        CustomPasswordHasher objCustomPasswordHasher = new CustomPasswordHasher();
                        //log.Info("Checked User is authorized.");
                        logInfoIsSessionId = "Checked User is authorized.";
                        ApplicationUser objUsers = new ApplicationUser();
                        objUsers.CreationDate = DateTime.Now;
                        objUsers.UpdateDate = DateTime.Now;
                        objUsers.BirthDate = DateTime.Now;
                        objUsers.UserName = objUser.UserName;
                        objUsers.Email = objUser.Email;
                        objUsers.FirstName = objUser.FirstName;
                        objUsers.LastName = objUser.LastName;
                        objUsers.PasswordHash = objCustomPasswordHasher.HashPassword(objUser.Password);
                        objUsers.SiteId = objUser.SiteId;
                        objUsers.AgeId = objUser.AgeId;
                        objUsers.GenderId = objUser.GenderId;
                        objUsers.MobileNumer = objUser.MobileNumer;
                        objUsers.GroupId = objUser.GroupName == 0 ? null : (int?)Convert.ToInt32(objUser.GroupName);


                        var result = UserManager.CreateAsync(objUsers);

                        if (result.Result.Succeeded)
                        {
                            UserManager.AddToRole(objUsers.Id, "WiFiUser");

                            //Save all the Users data in MySql DataBase
                            objRegisterDB.CreateNewUser(objUsers.UserName, objUsers.PasswordHash, objUsers.Email, objUsers.FirstName, objUsers.LastName);
                        }

                        retVal = Convert.ToInt32(ReturnCode.CreateUserSuccess);
                        retType = ReturnCode.Success.ToString();
                        retStr = "User created";
                        //dbContextTransaction.Commit();

                    }
                    else
                    {
                        retVal = Convert.ToInt32(ErrorCodeWarning.MacAddressorUserNameExist);
                        retStr = "Username already exists" + " " + objSite.SiteName;
                        retType = ReturnCode.Warning.ToString();
                    }
                }
                else
                {
                    retType = ReturnCode.Warning.ToString();
                }
                if (debugStatus == DebugMode.on.ToString())
                {
                    string logMsg = string.Concat(logInfoCreateUser, logInfoForCreateUserSuccess, logInfoIsSessionId, retStr);
                    log.Info(logMsg);
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                //dbContextTransaction.Rollback();
                retVal = Convert.ToInt32(ReturnCode.Failure);
                retType = ReturnCode.Warning.ToString();
                retStr = "Error Occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Error(retStr);
                }
            }

            objReturn.ReturnCode = retVal;
            objReturn.Message = retStr;
            objReturn.Type = retType;

            JavaScriptSerializer objSerialization = new JavaScriptSerializer();
            return new HttpResponseMessage()
            {
                Content = new StringContent(objSerialization.Serialize(objReturn), Encoding.UTF8, "application/json")
            };
            //}

        }

        [HttpPost]
        [Route("GetCompanyIcon")]
        public HttpResponseMessage CompanyIcon(int CompanyId)
        {

            if (CompanyId != 0)
            {
                var companyIcon = db.Company.FirstOrDefault(m => m.CompanyId == CompanyId).CompanyIcon;
                if (companyIcon != null)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(companyIcon, Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);

                }
            }
            else
            {
                string err = "CompanyId required.";
                return Request.CreateResponse(HttpStatusCode.OK, err);
            }

        }
    }
}
