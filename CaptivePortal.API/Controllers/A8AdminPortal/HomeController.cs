using CaptivePortal.API.Models;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers
{
    public class HomeController : Controller
    {

        A8AdminDbContext db = new A8AdminDbContext();
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(HomeController));
        private string retStr = "";
        private ApplicationUserManager _userManager;
        public HomeController()
        {

        }
        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
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
     
       [HttpGet]
        public ActionResult LogsDownload()
        {
            try
            {
                
                    string path = Server.MapPath("~/Logs/log.txt");
                    System.IO.FileInfo file = new System.IO.FileInfo(path);
                    if (file.Exists)
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                        return File(fileBytes, MediaTypeNames.Application.Octet, "log.txt");
                    }
               


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


        }

        //[Authorize(Roles = "GlobalAdmin,CompanyAdmin")]
        public ActionResult Index()
        {
            AdminlistViewModel list = new AdminlistViewModel();
            list.AdminViewlist = new List<AdminViewModel>();

            //int userId = User.Identity.GetUserId<int>();
            //string role = UserManager.GetRoles(userId).FirstOrDefault();
            //ViewBag.roleOfUser = role;
            //int siteId = Convert.ToInt32(db.Users.FirstOrDefault(m => m.Id == userId).SiteId);

            retStr = "entered in home to view overall estate";
            //int compId = 0;
            //int orgId = Convert.ToInt32(db.Company.FirstOrDefault(m => m.CompanyId == compId).Organisation.OrganisationId) == 0 ? 0 : Convert.ToInt32(db.Company.FirstOrDefault(m => m.CompanyId == compId).Organisation.OrganisationId);
            try
            {
                //if (siteId != 0)
                //{
                //    if (role == "CompanyAdmin")
                //    {
                //        compId = db.Site.FirstOrDefault(m => m.SiteId == siteId).Company.CompanyId;

                //        var accessSite = db.AdminSiteAccess.Where(m => m.UserId == userId).ToList();
                //        var accessSiteDetails = (from site in accessSite
                //                                 select new AdminViewModel()
                //                                 {
                //                                     // OrganisationName = db.Organisation.FirstOrDefault(m=>m.OrganisationId==orgId).OrganisationName,
                //                                     CompanyName = db.Company.FirstOrDefault(m => m.CompanyId == compId).CompanyName,
                //                                     SiteName = site.SiteName,
                //                                     DashboardUrl = db.Site.FirstOrDefault(m => m.SiteId == site.SiteId).DashboardUrl,
                //                                     RtlsUrl = db.Site.FirstOrDefault(m => m.SiteId == site.SiteId).RtlsUrl,
                //                                     DefaultSite = db.Users.FirstOrDefault(m => m.Id == userId).PhoneNumber,//default site to access
                //                                     SiteId = site.SiteId
                //                                 }).ToList();
                //        list.AdminViewlist.AddRange(accessSiteDetails);
                //    }
                //    else
                //    {

                //        compId = db.Site.FirstOrDefault(m => m.SiteId == siteId).Company.CompanyId;
                //        var result = db.Site.Where(m => m.CompanyId == compId).ToList();
                //        var siteDetails = (from item in result
                //                           select new AdminViewModel()
                //                           {
                //                               OrganisationName = item.Company.Organisation == null ? null : item.Company.Organisation.OrganisationName,

                //                               CompanyName = item.Company.CompanyName,
                //                               SiteName = item.SiteName,
                //                               DashboardUrl = item.DashboardUrl,
                //                               RtlsUrl = item.RtlsUrl,
                //                              
               //                                MySqlIpAddress = item.MySqlIpAddress,
                //                               SiteId = item.SiteId
                //                           }
                //                         ).ToList();
                //        list.AdminViewlist.AddRange(siteDetails);
                       
                //    }
                //}
                //else
                //{
                    var result = db.Site.ToList();

                    var siteDetails = (from item in result
                                       select new AdminViewModel()
                                       {
                                           OrganisationName = item.Company.Organisation == null ? null : item.Company.Organisation.OrganisationName,
                                           CompanyName = item.Company.CompanyName,
                                           SiteName = item.SiteName,
                                           DashboardUrl = item.DashboardUrl,
                                           RtlsUrl = item.RtlsUrl,
                                           SiteId = item.SiteId,
                                           MySqlIpAddress=item.MySqlIpAddress,
                                           CompanyId = item.Company == null ? null : item.Company.CompanyId.ToString()
                                       }
                                     ).ToList();
                    list.AdminViewlist.AddRange(siteDetails);
                }
            //}

            catch (Exception ex)
            {
                retStr = "some problem occured";
                throw ex;
            }
            return View(list);
        }
    }
}
