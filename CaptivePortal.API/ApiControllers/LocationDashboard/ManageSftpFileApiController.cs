using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using CaptivePortal.API.Models.LocationDashBoardModel;
using CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer;

namespace a8Dashboard.Controllers
{
    [RoutePrefix("DashBoard/api/ImportSftpData")]
    public class ManageSftpFileApiController : ApiController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateFileRepository objDateFileRepository = new DateFileRepository();
        private CruisedDiscoveryRepository objCruisedDiscoveryRepository = new CruisedDiscoveryRepository();


        [HttpGet]
        [Route("GetFileNames")]
        public HttpResponseMessage GetFileNames(string ConnectionString)
        {
            ConnectionString = Regex.Replace(ConnectionString.Trim(), @"\t|\n|\r", "");
            logger.Info("Entered GetReport()");
            string returnString = "";
            HttpResponseMessage returnResponse = new HttpResponseMessage();
            try
            {
                returnString = GenerateReturnJSON(objDateFileRepository.GetAllFileNames(ConnectionString), 1);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(returnString, System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("DeleteFileData")]
        public HttpResponseMessage DeleteFile(int Id, string ConnectionString)
        {
            ConnectionString = Regex.Replace(ConnectionString.Trim(), @"\t|\n|\r", "");
            logger.Info("Entered GetReport()");
            int returnCode = 0;
            HttpResponseMessage returnResponse = new HttpResponseMessage();
            ManageSftpRepository objManageSftp = new ManageSftpRepository();
            try
            {
                if (objManageSftp.DeleteFile(Id, ConnectionString) != -1)
                {
                    returnCode = 1;
                }
            }
            catch (Exception ex)
            {
                returnCode = -1;
                logger.Error(ex.Message);
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(returnCode.ToString(), System.Text.Encoding.UTF8, "application/json")
            };
        }


        /// <summary>
        /// Imoprt the TUI CruiseDiscoveryData excel file for knowing the Time Zone Difference
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveCruisePlaceDiscovery")]
        public HttpResponseMessage SaveCruisePlaceDiscovery()
        {
            try
            {
                logger.Info("Enter into the SaveCruisePlaceDiscovery()");
                string connection = HttpContext.Current.Request["ConnectionString"];
                HttpResponseMessage result = null;
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];

                        string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/CSV"), postedFile.FileName);
                        postedFile.SaveAs(filePath);
                        logger.Info("Save the spread sheet into the CSV Folder");
                        DataFileImporter objDataFileImporter = new DataFileImporter();
                        objDataFileImporter.ImportCruiseDiscoveryFile(filePath, connection);
                        File.Delete(filePath);
                        logger.Info("Delete the particular file after import");
                    }
                    result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }


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
