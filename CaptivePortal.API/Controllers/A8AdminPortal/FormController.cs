using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Repository;
using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.A8AdminPortal
{
    public class FormController : Controller
    {

        A8AdminDbContext db = new A8AdminDbContext();
        string debugStatus = ConfigurationManager.AppSettings["DebugStatus"];
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(FormController));
        private string retStr = "";
        FormControlRepository formControlRepo = new FormControlRepository();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="IsMandetory"></param>
        /// <returns></returns>
        /// POST: add new field for login and register page.
        [HttpPost]
        public JsonResult Save(FormControlViewModel model, string IsMandetory)
        {
            try
            {

                //FormControl
                formControlRepo.CreateFormControl(model, IsMandetory);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// DELETE:delete added field for register or login page.
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Form objForm = null;
            try
            {
                objForm = db.FormControl.FirstOrDefault(m => m.FormControlId == Id).Forms;
                formControlRepo.DeleteFormControl(Id);
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
            return RedirectToAction("ConfigureSite", "Admin", new { SiteId = objForm.SiteId });
        }
    }
}