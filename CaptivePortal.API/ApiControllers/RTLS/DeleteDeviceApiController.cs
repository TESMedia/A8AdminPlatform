using log4net;
using Newtonsoft.Json;
using CaptivePortal.API.Enums.RTLSEnums;
using CaptivePortal.API.Models;
using CaptivePortal.API.ViewModels.RTLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using CaptivePortal.API.Models.RTLSModel;
using RTLS.ViewModel;
using CaptivePortal.API.Models.A8AdminModel;

namespace CaptivePortal.API.ApiControllers.RTLS
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Device")]
    public class DeleteDeviceApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(DeleteDeviceApiController));
        private A8AdminDbContext db = new A8AdminDbContext();

        [Route("Delete")]
        [HttpPost]
        public HttpResponseMessage Delete(RequestLocationDataVM model)
        {
            Result objResult = new Result();
            string retResult = "";
            try
            {
                this.log.Debug("Enter into the DeleteMacAddress Action Method");
                foreach (var item in model.MacAddresses)
                {
                    var deviceObject = db.Device.FirstOrDefault(m => m.Mac == item);
                    if (deviceObject.Intstatus != Convert.ToInt32(DeviceStatus.Registered))
                    {
                        db.Device.Remove(deviceObject);
                        db.SaveChanges();
                        retResult = string.Format("{0} Successfully Deleted from server", item);
                    }
                    else
                    {
                        retResult = string.Format("{0} is a Registered User So shouldn't Delete", item);

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
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(retResult))
            };
        }
    }
}
