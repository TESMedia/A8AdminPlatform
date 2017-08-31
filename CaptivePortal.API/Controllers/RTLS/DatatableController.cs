using CaptivePortal.API.DataAccess.Repository.RTLS;
using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Web.Mvc;

namespace RTLS.Controllers
{
    public class DatatableController : Controller, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            try
            {
                param.iDisplayStart = Convert.ToInt32(Request["start"]);
                param.iDisplayLength= Convert.ToInt32(Request["length"]);
                param.sSearch = Request["search[value]"].ToString();
                using (LocationDataRepository objLocationDataRepository = new LocationDataRepository())
                {

                    int TotalRecords = objLocationDataRepository.CountLocationsData();
                    var result = objLocationDataRepository.GetListOfLocationData(param);
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = TotalRecords,
                        iTotalDisplayRecords = TotalRecords,
                        aaData = result
                    },
                      JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult AjaxHandlerToGetLogs(jQueryDataTableParamModel param)
        {
            try
            {
                using (AppLogRepository objAppLogRepo = new AppLogRepository())
                {
                    var result = objAppLogRepo.GetAppLogs(param);
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = objAppLogRepo.CountAppLogs(),
                        iTotalDisplayRecords = objAppLogRepo.CountAppLogs(),
                        aaData = result
                    },
                 JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public JsonResult AjaxHandlerGetDeviceData(string DeviceId)
        {
            try
            {
                try
                {
                    jQueryDataTableParamModel param = new jQueryDataTableParamModel();
                    param.iDisplayStart = Convert.ToInt32(Request["start"]);
                    param.iDisplayLength = Convert.ToInt32(Request["length"]);
                    param.MacAddress = DeviceId;
                    param.sSearch = Request["search[value]"].ToString();
                    using (LocationDataRepository objLocationDataRepository = new LocationDataRepository())
                    {

                        int TotalRecords = objLocationDataRepository.CountLocationsData();
                        var result = objLocationDataRepository.GetListOfLocationData(param);
                        return Json(new
                        {
                            sEcho = param.sEcho,
                            iTotalRecords = TotalRecords,
                            iTotalDisplayRecords = TotalRecords,
                            aaData = result
                        },
                          JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}