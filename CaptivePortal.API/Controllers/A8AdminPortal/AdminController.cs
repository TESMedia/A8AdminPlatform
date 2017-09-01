using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Models.CustomIdentity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Net.Mime;
using System.Net;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using CaptivePortal.API.Repository;
using CaptivePortal.API.Repository.CaptivePortal;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Controllers.CPAdmin
{
    public class AdminController : Controller
    {
        A8AdminDbContext db = new A8AdminDbContext();
        StringBuilder sb = new StringBuilder(String.Empty);
        FormControl objFormControl = new FormControl();
        string debugStatus = ConfigurationManager.AppSettings["DebugStatus"];
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(AdminController));
        private string retStr = "";
        private ApplicationUserManager _userManager;
        public AdminController()
        {

        }
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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
        public JsonResult GetRestrictedSite(int siteId)
        {
            int compId = db.Site.FirstOrDefault(m => m.SiteId == siteId).Company.CompanyId;
            var result = from item in db.Site.Where(m => m.CompanyId == compId).ToList()
                         select new
                         {
                             value = item.SiteId,
                             text = item.SiteName,
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Populate company list in dropdown.
        /// </summary>
        /// <returns>Company details</returns>
        ///Get:Create new site 
        public ActionResult CreateNewSite()
        {
            try
            {
                ViewBag.companies = from item in db.Company.ToList()
                                    select new SelectListItem()
                                    {
                                        Text = item.CompanyName,
                                        Value = item.CompanyId.ToString(),
                                    };
                retStr = "view company details";
                if (debugStatus == DebugMode.on.ToString())
                {
                    log.Info(retStr);
                }
            }
            catch (Exception ex)
            {
                retStr = "some problem occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Info(retStr);
                }
            }
            return View();
        }

        /// <summary>
        /// papulate organisation list in dropdown on select of company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public JsonResult GetOrganisations(int companyId)
        {
            var result = from item in db.Company.Where(m => m.CompanyId == companyId).ToList()
                         select new
                         {
                             value = item.Organisation == null ? 0 : item.Organisation.OrganisationId,
                             text = item.Organisation == null ? "" : item.Organisation.OrganisationName,
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult doesSiteNameExist(string SiteName)
        {
            var site = db.Site.Any(x => x.SiteName == SiteName);
            return Json(site);
        }
        public ActionResult CreatePromotionalMaterial(int? SiteId)
        {
            if (SiteId == null)
            {
                TempData["SiteIdCheck"] = "Please select any of the site and then manage promotional thing or If site is not there create new site";
                return RedirectToAction("Home", "Admin");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult StorePromotionalMaterial(ManagePromotion model)
        {
            string optionalPicturePath = null;
            string OptionalPictureForSuccessPage = null;
            ManagePromotion objManagePromotion = new ManagePromotion();
            var promo = db.ManagePromotion.ToList();
            if (promo.Count != 0)
            {
                var pro = db.ManagePromotion.Where(m => m.SiteId == model.SiteId).FirstOrDefault();
                if (pro != null)
                {
                    db.ManagePromotion.Remove(pro);
                    db.SaveChanges();
                }
                //image path
                if (Request.Files["OptionalPicture"].ContentLength > 0)
                {
                    var httpPostedFile = Request.Files["OptionalPicture"];
                    string savedPath = HostingEnvironment.MapPath("/Images/" + model.SiteId);
                    optionalPicturePath = "/Images/" + model.SiteId + "/" + httpPostedFile.FileName;
                    string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                    if (!System.IO.Directory.Exists(savedPath))
                    {
                        Directory.CreateDirectory(savedPath);
                    }
                    httpPostedFile.SaveAs(completePath);
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    OptionalPictureForSuccessPage = baseUrl + optionalPicturePath;
                }

                objManagePromotion.SiteId = model.SiteId;
                objManagePromotion.SuccessPageOption = model.SuccessPageOption;
                objManagePromotion.WebPageURL = model.WebPageURL;
                objManagePromotion.OptionalPictureForSuccessPage = OptionalPictureForSuccessPage;
                db.ManagePromotion.Add(objManagePromotion);
                db.SaveChanges();
            }
            else
            {
                //image path
                if (Request.Files["OptionalPicture"].ContentLength > 0)
                {
                    var httpPostedFile = Request.Files["OptionalPicture"];
                    string savedPath = HostingEnvironment.MapPath("/Images/" + model.SiteId);
                    optionalPicturePath = "/Images/" + model.SiteId + "/" + httpPostedFile.FileName;
                    string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                    if (!System.IO.Directory.Exists(savedPath))
                    {
                        Directory.CreateDirectory(savedPath);
                    }
                    httpPostedFile.SaveAs(completePath);
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    OptionalPictureForSuccessPage = baseUrl + optionalPicturePath;
                }

                objManagePromotion.SiteId = model.SiteId;
                objManagePromotion.SuccessPageOption = model.SuccessPageOption;
                objManagePromotion.WebPageURL = model.WebPageURL;
                objManagePromotion.OptionalPictureForSuccessPage = OptionalPictureForSuccessPage;
                db.ManagePromotion.Add(objManagePromotion);
                db.SaveChanges();
            }
            return RedirectToAction("Home", "Admin");
        }


        /// <summary>
        /// Create new site/org/comp/field.
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="fc"></param>
        /// <param name="dataType"></param>
        /// <param name="controlType"></param>
        /// <param name="fieldLabel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSiteAndLoginRegisterConf(FormViewModel inputData, FormCollection fc, FormControlViewModel model, string[] dataType, string[] controlType, string[] fieldLabel)
        {
            Site objSite = new Site();

            using (var dbContextTransaction = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    retStr = "entered to create new site";
                    string imagepath = null;
                    string bannerPath = null;
                    int orgId = inputData.organisationDdl;
                    string compId = inputData.CompanyDdl;
                    string fileName = null;
                    string TandD = null;

                    //organisation
                    OrganisationRepository organisationRepo = new OrganisationRepository();
                    organisationRepo.CreateOrganisation(inputData);

                    //company
                    CompanyRepository companyRepo = new CompanyRepository();
                    companyRepo.CreateCompany(inputData);

                    //Term and condition
                    if (Request.Files["Term_conditions"].ContentLength > 0)
                    {
                        var httpPostedFile = Request.Files["Term_conditions"];
                        string savedPath = HostingEnvironment.MapPath("/Upload/");
                        imagepath = "/Upload/" + httpPostedFile.FileName;
                        string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                        if (!System.IO.Directory.Exists(savedPath))
                        {
                            Directory.CreateDirectory(savedPath);
                        }
                        httpPostedFile.SaveAs(completePath);
                        fileName = httpPostedFile.FileName;
                        string name = httpPostedFile.ContentType;
                        if (httpPostedFile.ContentType == "application/pdf")
                        {
                            StringBuilder text = new StringBuilder();
                            using (PdfReader reader = new PdfReader(completePath))
                            {
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                                }
                            }
                            TandD = text.ToString();
                        }
                        else
                        {

                            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                            object miss = System.Reflection.Missing.Value;
                            object path = completePath;
                            object readOnly = true;
                            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                            string totaltext = "";
                            for (int i = 0; i < docs.Paragraphs.Count; i++)
                            {
                                totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
                            }

                            TandD = totaltext;
                        }

                    }
                    inputData.TermsAndCondDoc = TandD;

                    //site
                    SiteRepository siteRepo = new SiteRepository();
                    var result = siteRepo.CreateSite(inputData);

                    //image path
                    if (Request.Files["BannerIcon"].ContentLength > 0)
                    {
                        var httpPostedFile = Request.Files["BannerIcon"];
                        string savedPath = HostingEnvironment.MapPath("/Images/" + objSite.SiteId);
                        imagepath = "/Images/" + objSite.SiteId + "/" + httpPostedFile.FileName;
                        string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                        if (!System.IO.Directory.Exists(savedPath))
                        {
                            Directory.CreateDirectory(savedPath);
                        }
                        httpPostedFile.SaveAs(completePath);
                        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        bannerPath = baseUrl + imagepath;
                    }

                    FormRepository formRepo = new FormRepository();
                    formRepo.CreateForm(inputData);
                    dbContextTransaction.Commit();

                    if (debugStatus == DebugMode.on.ToString())
                    {
                        log.Info(retStr);
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    retStr = "some problem occured";
                    dbContextTransaction.Rollback();
                    if (debugStatus == DebugMode.off.ToString())
                    {
                        log.Info(retStr);
                    }

                    throw ex;
                }
            }
        }

        /// <summary>
        /// Show existing site details.
        /// </summary>
        /// <returns></returns>
        /// Get:Site details.
        public ActionResult SiteDetails()
        {
            var lstSite = (from item in db.Site.ToList()
                           select new SiteViewModel()
                           {
                               CmpName = item.Company.CompanyName,
                               OrgName = item.Company.Organisation.OrganisationName,
                               SiteName = item.SiteName,
                               SiteId = item.SiteId
                           }).ToList();
            return View(lstSite);
        }

        /// <summary>
        /// Populate Site details or form details of existing site Or create new org/comp/field/.
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public ActionResult ConfigureSite(int? SiteId)
        {
            FormViewModel objViewModel = new FormViewModel();
            try
            {
                if (SiteId != null)
                {
                    retStr = "populated selected site details to configure";
                    ViewBag.companies = from item in db.Company.ToList()
                                        select new SelectListItem()
                                        {
                                            Text = item.CompanyName,
                                            Value = item.CompanyId.ToString(),
                                        };
                    List<string> columnsList = db.Database.SqlQuery<string>("select column_name from information_schema.columns where table_name = 'users'").ToList();

                    Form objForm = db.Form.FirstOrDefault(m => m.SiteId == SiteId);
                    objForm.SiteId = Convert.ToInt32(SiteId);
                    objViewModel.SiteId = Convert.ToInt32(SiteId);
                    objViewModel.FormId = objForm.FormId;
                    objViewModel.SiteName = db.Site.FirstOrDefault(m => m.SiteId == SiteId).SiteName;
                    objViewModel.BannerIcon = objForm.BannerIcon;
                    objViewModel.BackGroundColor = objForm.BackGroundColor;
                    objViewModel.LoginWindowColor = objForm.LoginWindowColor;
                    objViewModel.IsPasswordRequire = objForm.IsPasswordRequire;
                    objViewModel.LoginPageTitle = objForm.LoginPageTitle;
                    objViewModel.AutoLogin = Convert.ToBoolean(objForm.Site.AutoLogin);
                    objViewModel.RegistrationPageTitle = objForm.RegistrationPageTitle;
                    objViewModel.ControllerIpAddress = db.Site.FirstOrDefault(m => m.SiteId == SiteId).ControllerIpAddress;
                    objViewModel.MySqlIpAddress = db.Site.FirstOrDefault(m => m.SiteId == SiteId).MySqlIpAddress;
                    objViewModel.Term_conditions = db.Site.FirstOrDefault(m => m.SiteId == SiteId).Term_conditions;
                    objViewModel.DashboardUrl = db.Site.FirstOrDefault(m => m.SiteId == SiteId).DashboardUrl;
                    objViewModel.RtlsUrl = db.Site.FirstOrDefault(m => m.SiteId == SiteId).RtlsUrl;

                    objViewModel.TermsAndCondDoc = db.Site.FirstOrDefault(m => m.SiteId == SiteId).TermsAndCondDoc;
                    objViewModel.fieldlabel = columnsList;
                    if (db.Site.Any(m => m.SiteId == SiteId))
                    {
                        objViewModel.CompanyDdl = db.Site.FirstOrDefault(m => m.SiteId == SiteId).CompanyId.ToString();
                    }
                    objViewModel.FormControls = db.FormControl.Where(m => m.FormId == objForm.FormId).ToList();

                    if (debugStatus == DebugMode.on.ToString())
                    {
                        log.Info(retStr);
                    }
                }
                else
                {
                    TempData["SiteIdCheck"] = "Please select any of the site you want to cofigure if site is not available please create new site and configure";
                    return RedirectToAction("Home", "Admin");
                }
            }
            catch (Exception ex)
            {
                retStr = "some problem occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Info(retStr);
                }
                throw ex;
            }
            return View(objViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(FormCollection fc)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var file = Request.Files[fc["BannerIcon"]];
                    byte[] fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, file.ContentLength);
                    if (debugStatus == DebugMode.on.ToString())
                    {
                        log.Info(retStr);
                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    retStr = "some problem occured";
                    if (debugStatus == DebugMode.on.ToString())
                    {
                        log.Info(retStr);
                    }
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// On Update site Detail Submit
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSiteAndLoginRegisterConf(FormViewModel inputData, FormCollection fc, FormControlViewModel model, string[] fieldLabel)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    retStr = "entered to configure site";
                    if (inputData.CompanyName == null)
                    {
                        string imagepath = null;
                        string bannerPath = null;
                        string filePath = null;
                        string fileName = null;
                        string TandD = null;
                        string compId = inputData.CompanyDdl;
                        if (Request.Files["BannerIcon"].ContentLength > 0)
                        {
                            var httpPostedFile = Request.Files["BannerIcon"];
                            string savedPath = HostingEnvironment.MapPath("/Images/" + inputData.SiteId);
                            imagepath = "/Images/" + inputData.SiteId + "/" + httpPostedFile.FileName;
                            string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                            if (!System.IO.Directory.Exists(savedPath))
                            {
                                Directory.CreateDirectory(savedPath);
                            }
                            httpPostedFile.SaveAs(completePath);
                            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                            bannerPath = baseUrl + imagepath;
                        }
                        else
                        {
                            bannerPath = inputData.BannerIcon;
                        }

                        //Term and condition
                        if (Request.Files["Term_conditions"].ContentLength > 0)
                        {
                            var httpPostedFile = Request.Files["Term_conditions"];
                            string savedPath = HostingEnvironment.MapPath("/Upload/");
                            filePath = "/Upload/" + httpPostedFile.FileName;
                            string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                            if (!System.IO.Directory.Exists(savedPath))
                            {
                                Directory.CreateDirectory(savedPath);
                            }
                            httpPostedFile.SaveAs(completePath);
                            fileName = httpPostedFile.FileName;

                            if (httpPostedFile.ContentType == "application/pdf")
                            {
                                StringBuilder text = new StringBuilder();
                                using (PdfReader reader = new PdfReader(completePath))
                                {
                                    for (int i = 1; i <= reader.NumberOfPages; i++)
                                    {
                                        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                                    }
                                }
                                TandD = text.ToString();
                            }
                            else
                            {

                                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                                object miss = System.Reflection.Missing.Value;
                                object path = completePath;
                                object readOnly = true;
                                Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
                                string totaltext = "";
                                for (int i = 0; i < docs.Paragraphs.Count; i++)
                                {
                                    totaltext += " \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString();
                                }

                                TandD = totaltext;
                            }
                        }
                        else
                        {
                            TandD = inputData.TermsAndCondDoc;
                        }

                        //site
                        SiteRepository siteRepo = new SiteRepository();
                        siteRepo.UpdateSite(inputData);

                        //form
                        FormRepository formRepo = new FormRepository();
                        formRepo.UpdateForm(inputData);

                        dbContextTransaction.Commit();

                    }
                    if (debugStatus == DebugMode.on.ToString())
                    {
                        log.Info(retStr);
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    retStr = "some problem occured";
                    if (debugStatus == DebugMode.off.ToString())
                    {
                        log.Info(retStr);
                    }
                    throw ex;
                }
                return RedirectToAction("Home", "Admin");
            }
        }


        [HttpPost]
        public JsonResult SaveFormControls(FormControlViewModel model, string IsMandetory)
        {
            try
            {
                Form objForm = db.Form.FirstOrDefault(m => m.FormId == model.FormId);

                //FormControl
                FormControlRepository formControlRepo = new FormControlRepository();
                formControlRepo.CreateFormControl(model,IsMandetory);
                if (debugStatus == DebugMode.on.ToString())
                {
                    log.Info(retStr);
                }
            }
            catch (Exception ex)
            {
                retStr = "some problem occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Info(retStr);
                }
                return Json("Failure");
            }
            return Json("Success");
        }

        [HttpGet]
        public ActionResult DeleteFormControl(int Id)
        {
            FormControl objFormControl = null;
            Form objForm = null;
            try
            {
                objFormControl = db.FormControl.FirstOrDefault(m => m.FormControlId == Id);
                objForm = db.FormControl.FirstOrDefault(m => m.FormControlId == Id).Forms;
                db.FormControl.Remove(objFormControl);
                db.SaveChanges();
                if (debugStatus == DebugMode.on.ToString())
                {
                    log.Info(retStr);
                }
            }
            catch (Exception ex)
            {
                retStr = "some problem occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Info(retStr);
                }
            }
            return RedirectToAction("ConfigureSite", new { SiteId = objForm.SiteId });
        }

        
        // GET: AdminIndex
        public ActionResult Index()
        {
            ViewBag.sites = from item in db.Site.ToList()
                            select new SelectListItem()
                            {
                                Value = item.SiteId.ToString(),
                                Text = item.SiteName
                            };
            return View();
        }

        

        public ActionResult UploadFile(int? siteId)
        {
            if (siteId != 0 && siteId != null)
            {
                return View();
            }
            else
            {
                TempData["SiteIdCheck"] = "Please select any of the site and then upload";
                return RedirectToAction("Home", "Admin");
            }
        }

        public ActionResult Locations(int? siteId)
        {
            if (siteId != 0 && siteId != null)
            {
                return View();
            }
            else
            {
                TempData["SiteIdCheck"] = "Please select any of the site and then got location mapping";
                return RedirectToAction("Home", "Admin");
            }
        }

        public ActionResult Locationdashboard()
        {
            return View();
        }

        public JsonResult GetCompany(int orgId)
        {
            var result = from item in db.Company.Where(m => m.CompanyId == orgId).ToList()
                         select new
                         {
                             value = item.CompanyId,
                             text = item.CompanyName
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSite(int compId)
        {
            var result = from item in db.Site.Where(m => m.CompanyId == compId).ToList()
                         select new
                         {
                             value = item.SiteId,
                             text = item.SiteName
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        

       

        public ActionResult AddMacAddress(FormCollection fc)
        {
            int userId = Convert.ToInt16(fc["UserId"]);
            var objUser = db.Users.Find(userId);
            {

                MacAddress mac = new MacAddress();
                mac.MacAddressValue = fc["MacAddress"];
                mac.UserId = userId;
                db.MacAddress.Add(mac);
                //db.Entry(objUser).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("UserDetails", "Admin");

        }

        [HttpPost]
        public ActionResult DeleteMacAddress(int MacId)
        {
            MacAddress objMac = db.MacAddress.Find(MacId);
            {
                db.MacAddress.Remove(objMac);
                db.SaveChanges();
            }
            return RedirectToAction("UserDetails", "Admin");
        }

        [HttpGet]
        public ActionResult LogsDownload()
        {
            try
            {
                retStr = "download log file";
                string path = Server.MapPath("~/Logs/log.txt");
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                    return File(fileBytes, MediaTypeNames.Application.Octet, "log.txt");
                }
                if (debugStatus == DebugMode.on.ToString())
                {
                    log.Info(retStr);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                retStr = "some problem occured";
                if (debugStatus == DebugMode.off.ToString())
                {
                    log.Info(retStr);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult TestSetUpRtls()
        {
            return View();
        }

        public ActionResult ViewRtlsData()
        {
            return View();
        }

        public ActionResult ViewErrorLogRtls()
        {
            return View();
        }

    }
}