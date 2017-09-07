using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using log4net;
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
        public AdminController()
        {

        }

        /// <summary>
        /// Populate company list in dropdown.
        /// </summary>
        /// <returns>Company details</returns>
        ///Get:Create new site Page. 
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
                throw ex;
            }
            return View();
        }

        /// <summary>
        /// papulate organisation list in dropdown on select of company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// GET
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <returns></returns>
        /// POST:check site name already exist ?
        [HttpPost]
        public JsonResult doesSiteNameExist(string SiteName)
        {
            var site = db.Site.Any(x => x.SiteName == SiteName);
            return Json(site);
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
        /// POST:configuration for site and Login register form.
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
                   // int orgId = inputData.organisationDdl;
                    string compId = inputData.CompanyDdl;
                    string fileName = null;
                    string TandD = null;

                    //organisation
                    OrganisationRepository organisationRepo = new OrganisationRepository();
                    organisationRepo.CreateOrganisation(inputData);

                    //orgId=db.

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
                    //horm
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
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// GET:get accessable site for company admin which site is permitted by Global admin.
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
        /// GET:Configure site Page.
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
                    objViewModel.Fieldlabel = columnsList;
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
                    return RedirectToAction("Index", "Home");
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

        /// <summary>
        /// On Update site Detail Submit
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="fc"></param>
        /// <returns></returns> 
        /// POST:update site/org/comp, icon
        [HttpPost]
        public ActionResult UpdateSiteAndLoginRegisterConf(FormViewModel inputData, FormCollection fc, FormControlViewModel model, string[] FieldLabel)
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
                        string companyIconPath = null;

                        string fileName = null;
                        string TandD = null;
                        int compId = Convert.ToInt32(inputData.CompanyDdl);
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

                        if (Request.Files["CompanyIcon"].ContentLength > 0)
                        {
                            var httpPostedFile = Request.Files["CompanyIcon"];
                            string savedPath = HostingEnvironment.MapPath("/Images/" + inputData.SiteId);
                            imagepath = "/Images/" + compId + "/" + httpPostedFile.FileName;
                            string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                            if (!System.IO.Directory.Exists(savedPath))
                            {
                                Directory.CreateDirectory(savedPath);
                            }
                            httpPostedFile.SaveAs(completePath);
                            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                            companyIconPath = baseUrl + imagepath;
                        }
                        else
                        {
                            companyIconPath = inputData.CompanyIcon;
                        }

                        //site
                        SiteRepository siteRepo = new SiteRepository();
                        siteRepo.UpdateSite(inputData);

                        //form
                        FormRepository formRepo = new FormRepository();
                        formRepo.UpdateForm(inputData);

                        CompanyRepository compRepo = new CompanyRepository();
                        compRepo.UpdateCompany(inputData); 

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
                return RedirectToAction("Index", "Home");
            }
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Wi-Fi summary page.
        /// </summary>
        /// <returns></returns>
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

        public ActionResult Locationdashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadLocationDashBoardFile(string SiteName)
        {
            if (!String.IsNullOrEmpty(SiteName))
            {
                return View();
            }
            else
            {
                TempData["SiteIdCheck"] = "Please select any of the site and then upload";
                return RedirectToAction("Home", "Admin");
            }
        }

    }
}