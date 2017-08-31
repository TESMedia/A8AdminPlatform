//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http;
//using WinSCP;
//using log4net;
//using System.Text.RegularExpressions;
//using CaptivePortal.API.Models.LocationDashBoardModel;
//using CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer;

//namespace a8Dashboard.Controllers
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [RoutePrefix("Dashboard/api/ImportSftpData")]
//    public class ImportSftpDataController : ApiController
//    {
//        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//        private string retMessage;


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="lstFileName"></param>
//        /// <param name="ConnectionString"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("ImportCSVFile")]
//        public HttpResponseMessage GetDataFromSmtp(string lstDataFileIds, string ConnectionString)
//        {
//            DataFileImporter objDataFileImporter = new DataFileImporter();
//            try
//            {
//                string connectionstring = Regex.Replace(ConnectionString.Trim(), @"\t|\n|\r", "");
//                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(ConnectionString))
//                {
//                    foreach (var item in lstDataFileIds.Split(','))
//                    {
//                        int idItem = Convert.ToInt32(item);
//                        string pathName = db.DataFiles.Where(m => m.Id == idItem).FirstOrDefault().FileName;
//                        string fileName = Path.GetFileName(pathName);
//                        objDataFileImporter.SaveDataFile(pathName, fileName, ConnectionString);
//                        retMessage = "successfully Saved";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                retMessage = ex.InnerException.Message;
//                throw ex;
//            }

//            return new HttpResponseMessage()
//            {
//                Content = new StringContent(retMessage)
//            };
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="lstDataFileIds"></param>
//        /// <param name="ConnectionString"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("ClearFile")]
//        public HttpResponseMessage ClearFileData(string lstDataFileIds, string ConnectionString)
//        {
//            DataFileImporter objDataFileImporter = new DataFileImporter();
//            try
//            {
//                string connectionstring = Regex.Replace(ConnectionString.Trim(), @"\t|\n|\r", "");
//                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(ConnectionString))
//                {
//                    foreach (var item in lstDataFileIds.Split(','))
//                    {
//                        int idItem = Convert.ToInt32(item);

//                        DataFile objdataFile = db.DataFiles.Find(idItem);
//                        db.DataFiles.Remove(objdataFile);
//                        db.SaveChanges();
//                        retMessage = "successfully Deleted";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                retMessage = ex.InnerException.Message;
//                throw ex;
//            }

//            return new HttpResponseMessage()
//            {
//                Content = new StringContent(retMessage)
//            };
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="strDateFormat"></param>
//        /// <param name="ConnectionString"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("LoadSftpData")]
//        public HttpResponseMessage LoadSftpData(string strDateFormat, string ConnectionString)
//        {
//            try
//            {
//                DataFileImporter objDataFileImporter = new DataFileImporter();
//                List<DataFile> lstDataFile = new List<DataFile>();
//                ConnectionString = Regex.Replace(ConnectionString.Trim(), @"\t|\n|\r", "");
//                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(ConnectionString))
//                {
//                    Session session = new Session();
//                    string path = db.Parameters.FirstOrDefault(m => m.Name == "RemotePath").Value;
//                    string localFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/ZipCsvs");
//                    SessionOptions sessionOptions = new SessionOptions
//                    {
//                        Protocol = Protocol.Sftp,
//                        HostName = ConfigurationManager.AppSettings["HostName"],
//                        UserName = ConfigurationManager.AppSettings["UserName"],
//                        Password = ConfigurationManager.AppSettings["Password"],
//                    };
//                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
//                    session.Open(sessionOptions);
//                    string remoteFilePath = db.Parameters.FirstOrDefault(m => m.Name == "RemotePath").Value + "/" + strDateFormat + "/" + "cleaned";
//                    if (session.FileExists(remoteFilePath))
//                    {
//                        //Fetch all the Files from SFTP as per configuration structure setting in Config
//                        var SftpFiles = session.EnumerateRemoteFiles(remoteFilePath, "", EnumerationOptions.None).Where(m => m.Name.Contains(ConfigurationManager.AppSettings["csvFileNameFormat1"].ToString()) || m.Name.Contains(ConfigurationManager.AppSettings["csvFileNameFormat2"].ToString()) || m.Name.Contains(ConfigurationManager.AppSettings["csvFileNameFormat3"].ToString()));
//                        foreach (var item1 in SftpFiles)
//                        {
//                            //Check the DataFile exist or not in database for duplicate
//                            if (!db.DataFiles.Any(m => (m.FileName == item1.FullName) || (m.FileName == item1.Name)))
//                            {
//                                DataFile objDataFile = new DataFile();
//                                objDataFile.FileName = item1.FullName;
//                                objDataFile.DateOfFile = Convert.ToDateTime(objDataFileImporter.GetDateFromString(item1.Name));
//                                objDataFile.IsInSftp = true;
//                                lstDataFile.Add(objDataFile);

//                            }

//                        }
//                    }
//                    if (lstDataFile.Count > 0)
//                    {
//                        //Save the changes for the new collection of DataFiles
//                        db.DataFiles.AddRange(lstDataFile);
//                        db.SaveChanges();
//                    }
//                    retMessage = "Sucessfully Imported";
//                }
//            }
//            catch (Exception ex)
//            {
//                retMessage = ex.Message;
//                throw ex;
//            }
//            return new HttpResponseMessage()
//            {
//                Content = new StringContent(retMessage)
//            };
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        /// <param name="ConnectionString"></param>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("UpdateParameters")]
//        public HttpResponseMessage UpdateParameters(string key, string value, string ConnectionString)
//        {
//            try
//            {
//                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(ConnectionString))
//                {
//                    Parameter objParameter = db.Parameters.FirstOrDefault(m => m.Name == key);
//                    objParameter.Value = value;
//                    db.Entry(objParameter).State = System.Data.Entity.EntityState.Modified;
//                    db.SaveChanges();
//                    return new HttpResponseMessage()
//                    {
//                        Content = new StringContent("successfully Updated")
//                    };
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex.Message);
//                throw ex;
//            }
//        }
//    }
//}

