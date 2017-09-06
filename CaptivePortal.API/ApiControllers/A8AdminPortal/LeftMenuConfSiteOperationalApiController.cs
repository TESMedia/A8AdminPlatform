using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace CaptivePortal.API.ApiControllers.A8AdminPortal
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("MenuConfSite")]
    public class LeftMenuConfSiteOperationalApiController : ApiController
    {
        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(LeftMenuConfSiteOperationalApiController));

        private A8AdminDbContext db = new A8AdminDbContext();
        

        [HttpPost]
        [Route("a8Captiveportal/V1/SiteOperationalStatus")]
        public HttpResponseMessage OperationalStatus(LoginWIthNewMacAddressModel model)
        {


            var siteDetails = db.Site.FirstOrDefault(m => m.SiteId == model.SiteId);
            var replyMessage = "";
            List<string> pingList = new List<string>();
            string[] data = new string[4];
            data[0] = siteDetails.ControllerIpAddress;
            data[1] = siteDetails.MySqlIpAddress;
            data[2] = siteDetails.RtlsUrl;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Ping myPing = new Ping();
                    if (data[i] != null)
                    {
                        PingReply reply = myPing.Send(data[i], 1000);
                        if (reply != null)
                        {
                            replyMessage = reply.Status.ToString();
                        }
                    }
                    else
                    {
                        replyMessage = "NotDeployed";

                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                pingList.Add(replyMessage);
            }

            SqlParameter parameter1 = new SqlParameter("@SiteId", model.SiteId);
            // int result = db.Database.SqlQuery<int>("Exec GetSessionDataPerSite @siteId", parameter1).First();
            string filepath = HttpContext.Current.Server.MapPath("~/Logs/log.txt");
            var lineCount = File.ReadLines(filepath).Count();

            //  pingList.Add(result.ToString());
            pingList.Add(lineCount.ToString());
            pingList.Add(siteDetails.RtlsUrl);
            pingList.Add(siteDetails.DashboardUrl);
            pingList.Add(siteDetails.MySqlIpAddress);

            JavaScriptSerializer objSerialization = new JavaScriptSerializer();
            return new HttpResponseMessage()
            {
                Content = new StringContent(objSerialization.Serialize(pingList), Encoding.UTF8, "application/json")
            };
        }


    }
}
