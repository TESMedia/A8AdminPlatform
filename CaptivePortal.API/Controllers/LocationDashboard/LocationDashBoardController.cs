using log4net;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;
using CaptivePortal.API.Models.LocationDashBoardModel;
using CaptivePortal.API.DataAccess.Repository.A8AdminPortal;
using Microsoft.AspNet.Identity;
using CaptivePortal.API.Repository.CaptivePortal;
using CaptivePortal.API.Models.CustomIdentity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace CaptivePortal.API.Controllers.LocationDashboard
{
    /// <summary>
    /// Controller for handling all the Operation of Dashboard page Specially URL Routing
    /// </summary>
    public class LocationDashBoardController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AdminSiteAccessRepository objAdminSiteAccessRepository;
        private SiteRepository objSiteRepository;

        public LocationDashBoardController()
        {
            objAdminSiteAccessRepository = new AdminSiteAccessRepository();
            objSiteRepository = new SiteRepository();
        }

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// Dashboard Home page for User
        /// </summary>
        /// <param name="SiteName">Passing the SiteName as QueryString</param>
        /// <returns></returns>

        [Authorize(Roles = "GlobalAdmin,BusinessUser,User")]
        public ActionResult Index(int SiteId)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var objSite=objSiteRepository.GetSiteAsPerId(SiteId);
           
            if (userManager.IsInRole(User.Identity.GetUserId<int>(), "GlobalAdmin"))
            {
                ViewData["SiteList"] =new SelectList(objSiteRepository.GetListOfSiteGroupPerSiteId(objSite.SiteId).Select(m=>new {SiteName=m.SiteName,SiteId=m.SiteName}),"SiteId","SiteName", objSite.SiteName.Trim()) ;
            }
            else
            {
                ViewData["SiteList"] = new SelectList(objAdminSiteAccessRepository.GetListOfAdminSiteAccess(User.Identity.GetUserId<int>()).Select(m => new { SiteName = m.Site.SiteName, SiteId = m.Site.SiteName }), "SiteId", "SiteName", objSite.SiteName.Trim());
            }
            ViewData["SiteImage"] = objSite.SiteIconPath;
            ViewData["SiteName"] = objSite.SiteName;
            return View();
        }

        
        /// <summary>
        /// Only Global Admin Accessible Page to Import the data from SFTP and store in database 
        /// </summary>
        /// <param name="SiteName">Querystring for getting the Data from Different Site</param>
        /// <returns></returns>
        [Authorize(Roles = "GlobalAdmin")]
        public ActionResult UploadFile(string SiteName)
        {
            using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(SiteName.ToString().Trim()))
            {
                ViewBag.DeltaTime = db.Parameters.FirstOrDefault(m => m.Name == "DeltaTime").Value;
                ViewBag.RemotePath = db.Parameters.FirstOrDefault(m => m.Name == "RemotePath").Value;
                ViewBag.WindowCovCalcDwellTime = db.Parameters.FirstOrDefault(m => m.Name == "WindowConvDwellTime").Value;
                ViewBag.WindowConvLengthTime = db.Parameters.FirstOrDefault(m => m.Name == "WindowConvLengthTime").Value;
                ViewBag.SiteName = SiteName;
                return View();
            }
        }


        /// <summary>
        /// Download the Logs to see the debug with Error
        /// </summary>
        /// <returns></returns>
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
    }
}
