using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mime;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.A8AdminPortal
{
    public class LogsController : Controller
    {
        string debugStatus = ConfigurationManager.AppSettings["DebugStatus"];
        private string retStr = "";
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(LogsController));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
    }
}