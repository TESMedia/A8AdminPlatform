using CaptivePortal.API.Models.RTLSModel;
using CaptivePortal.API.ViewModels.RTLS;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RTLS.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("UIData")]
    public class ViewDataApiController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger(typeof(ViewDataApiController));
        private RTLSDbContext db = new RTLSDbContext();

        [Route("ListOfMacAddress")]
        [HttpGet]
        public HttpResponseMessage GetListOfMacAddress()
        {
            PagedResults objPagedResults = new PagedResults();
            objPagedResults.currentPageIndex=  1;

            try
            {
                var Maclist = db.MacAddress.ToList();
                objPagedResults.PageSize = Maclist.Count();
                objPagedResults.TotalPages= (int)Math.Ceiling((decimal)Maclist.Count / (decimal)objPagedResults.PageSize);
                objPagedResults.lstMacAddress.AddRange(Maclist);
            }
            catch (Exception ex)
            {
                log.Error(ex.InnerException.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(objPagedResults), Encoding.UTF8, "application/json")
            };
        }

    }
}
