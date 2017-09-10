using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using log4net;
using RTLS.ViewModel;
using System.Web.Mvc;
using CaptivePortal.API.Models.RTLSModel;
using CaptivePortal.API.DataAccess.Repository.RTLS;
using CaptivePortal.API.ViewModels.RTLS;
using CaptivePortal.API.Enums.RTLSEnums;

namespace RTLS.Controllers
{
    public class RTLSController : Controller
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(RTLSController));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            List<Device> ListOfDevices = null;
            try
            {
                using (DeviceRepository objMacAddressRepository = new DeviceRepository())
                {
                    ListOfDevices = objMacAddressRepository.GetListOfMacAddress();
                    //using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
                    //{
                    //    objRequestLocationDataVM.RtlsConfiguration = objRtlsConfigurationRepository.GetRtlsConfigurationAsPerSite(SiteId);
                    //    objRequestLocationDataVM.RtlsConfiguration.SiteId = SiteId;
                    //    ViewBag.SiteName = objRtlsConfigurationRepository.GetSiteName(SiteId);
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException);
            }
            return View(ListOfDevices);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="chkMacDevices"></param>
        /// <param name="txtMacDevices"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(RequestLocationDataVM model)
        {
            try
            {
                using (DeviceRepository objMacRepository = new DeviceRepository())
                {
                    if (objMacRepository.CheckListExistOrNot(model.MacAddresses))
                    {
                        objMacRepository.SaveMacAddress(model.MacAddresses, true);
                        //using (RtlsConfigurationRepository objRtlsConfigurationRepository = new RtlsConfigurationRepository())
                        //{
                        //    objRtlsConfigurationRepository.AddUpdateRtlsConfiguration(model.RtlsConfiguration);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return RedirectToAction("Admin","TestSetUp",new {SiteId=model.RtlsConfiguration.SiteId});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewMonitorDevices()
        {
            MonitorDevices objMonitorDevice = null;
            string strResult = null;
            try
            {
                this.log.Debug("Enter into the ViewMonitorDevices Action Method");
                //EngageLocations objApiCall = new EngageLocations();
                //string strResult = objApiCall.GetAllDeviceDetails();
                if (!string.IsNullOrEmpty(strResult))
                {
                    objMonitorDevice = JsonConvert.DeserializeObject<MonitorDevices>(strResult);
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.Message);
            }
            return View(objMonitorDevice.records);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public ActionResult RTLSDataAsDevice(string DeviceId)
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RTLSRegisteredData()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRTLSLogs()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMacAddress(RequestLocationDataVM model)
        {
            Result objResult = new Result();
            string retResult = "";
            try
            {
                this.log.Debug("Enter into the DeleteMacAddress Action Method");
                foreach (var item in model.MacAddresses)
                {
                    using (DeviceRepository objMacAddressRepository = new DeviceRepository())
                    {
                        var deviceObject = objMacAddressRepository.GetDeviceFromMac(item);
                        if (deviceObject.Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                        {
                            objMacAddressRepository.DeleteMacAddress(deviceObject);
                            retResult = string.Format("{0} Successfully Deleted from server", item);
                        }
                        else
                        {
                            retResult = string.Format("{0} is a Registered User So shouldn't Delete", item);

                        }
                    }
                       
                }
            }
            catch (Exception ex)
            {
                this.log.Error("Exception occur" + ex.InnerException.Message);
                retResult = "Exception occur" + ex.InnerException.Message;
                objResult.returncode = -1;
            }
            objResult.errmsg = retResult;
            return Json(JsonConvert.SerializeObject(retResult), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objMacAddress"></param>
        [HttpPost]
        public void UpdateIsTracking(Device objMacAddress)
        {
            using (DeviceRepository objMacAddressRepository = new DeviceRepository())
            {
                var objMac = objMacAddressRepository.GetDeviceFromMac(objMacAddress.Mac);
                objMac.IsTracking = objMacAddress.IsTracking;
                objMacAddressRepository.UpdateMacAddres(objMac);
            }
               
        }
    }
}