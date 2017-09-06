using CaptivePortal.API.Repository.CaptivePortal;
using System.Web.Mvc;
using System;

namespace CaptivePortal.API.Controllers.A8AdminPortal
{
    public class MacAddressSetByAdminController : Controller
    {
        MacAddressRepository macRepo = new MacAddressRepository();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        public ActionResult AddMacAddress(FormCollection fc)
        {
            try
            {
                macRepo.AddMacAddressForWiFiUser(Convert.ToInt16(fc["UserId"]), fc["MacAddress"]);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("UserDetails", "Admin");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MacId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMacAddress(int MacId)
        {
            try
            {
                macRepo.DleteMacAddressOfWiFiUser(MacId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("UserDetails", "Admin");
        }
        
    }
}