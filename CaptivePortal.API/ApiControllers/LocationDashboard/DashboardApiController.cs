using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using CaptivePortal.API.ViewModels.LocationDashBoard;
using CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer;

namespace a8Dashboard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("DashBoard/api")]
    public class DashboardApiController : ApiController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the result from the Dashboard as per the Search from User
        /// </summary>
        /// <param name="iReportParms"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateReport")]
        public HttpResponseMessage GenerateReport(ReportViewModel iReportParms)
        {
            logger.Info("Entered GetReport()");
            string returnString = "";
            HttpResponseMessage returnResponse = new HttpResponseMessage();
            try
            {
                ReportRepository objReportRepository = new ReportRepository();
                int retCode = objReportRepository.GenerateReport(iReportParms);
                if (retCode == -1)
                    throw new Exception(objReportRepository.ErrorString);
                else
                    returnString = GenerateReturnJSON(objReportRepository.GetJSON(), 1);

            }
            catch (Exception ex)
            {
                returnString = GenerateReturnJSON("{\"Error\":\"Sorry, an error occured and we are unable to generate a report\"}", -1);
                logger.Error("Error in GetReport() - " + ex.Message);
            }

            logger.Info("Leaving GetReport()");
            //JavaScriptSerializer ser = new JavaScriptSerializer();
            //string ret = ser.Serialize(returnString);
            //ret = ret.Replace(@"\", "");
            //ret = ret.Remove(0, 1);
            //ret = ret.TrimEnd('"');
            return new HttpResponseMessage()
            {
                Content = new StringContent(returnString, System.Text.Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// Get all the AreaOfInterest as per the Site
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetLocation")]
        public HttpResponseMessage GetAreaOfInterest(string ConnectionString)
        {
            logger.Info("Entered GetReport()");
            string returnString = "";
            HttpResponseMessage returnResponse = new HttpResponseMessage();
            try
            {
                AreaOfInterestRepository objInterestLocation = new AreaOfInterestRepository();
                int retCode = objInterestLocation.GetAllAreaOfInterst(ConnectionString);
                if (retCode == -1)
                    throw new Exception(objInterestLocation.ErrorString);
                else
                    returnString = GenerateReturnJSON(objInterestLocation.GetJSON(), 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(returnString, System.Text.Encoding.UTF8, "application/json")
            };
        }

        /// <summary>
        /// Get All the Date of Imported file exist in database
        /// </summary>
        /// <param name="ConnectionString">Connection string as per SiteName</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDate")]
        public HttpResponseMessage GetDate(string ConnectionString)
        {
            logger.Info("Entered GetReport()");
            string returnString = "";
            HttpResponseMessage returnResponse = new HttpResponseMessage();
            try
            {
                DateFileRepository objDataFile = new DateFileRepository();
                int retCode = objDataFile.GetAllDateFromDB(ConnectionString);
                if (retCode == -1)
                    throw new Exception(objDataFile.ErrorString);
                else
                    returnString = GenerateReturnJSON(objDataFile.GetJSON(), 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(returnString, System.Text.Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// Generate the Json from the Content data
        /// </summary>
        /// <param name="contentString">String of Data</param>
        /// <param name="returnCode">The code for Success or Failure</param>
        /// <returns></returns>
        private string GenerateReturnJSON(string contentString, int returnCode)
        {
            string JSONString = "[{" +
                                    "\"ReturnCode\":\"" + returnCode.ToString() + "\"," +
                                    "\"Content\":" + contentString +
                                "}]";
            return JSONString;
        }
    }
}


