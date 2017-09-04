using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.A8AdminPortal
{
    public class UploadFileController : Controller
    {
        string debugStatus = ConfigurationManager.AppSettings["DebugStatus"];
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(UploadFileController));
        private string retStr = "";
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
    }
}