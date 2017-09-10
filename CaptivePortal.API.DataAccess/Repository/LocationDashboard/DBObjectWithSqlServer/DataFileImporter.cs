using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WebGrease.Configuration;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Web.Configuration;
using Excel;
using WinSCP;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    /* Imports a day's worth of data into the database in the following steps
     * 1. FTP the zipped up file to the local server
     * 2. Unzip the file
     * 3. Run the stored proc to import the data, remove unwanted MAC addresses and duplicate lines
     * 4. Delete file from the local server
     */
    public class DataFileImporter : DBObject
    {
        string CrusieDicoveryTableName = "dbo.CruisedDiscovery";
        string FullUnzippedFilename = "";
        string ZippedFilename = "";
        string TargetDay = "";
        public DataFileImporter()
        { }

        // Return 1 on success, 0 if file has already been imported and -1 on error
        public int SaveDataFile(string iFilename,string fileName,string connectionName)
        {
            ZippedFilename = fileName;
            TargetDay = GetDateFromString(fileName);
            logger.Info("Entered ImportDataFile(" + ZippedFilename + ")");

            int retCode = 0;
            ErrorString = "";

            try
            {
                logger.Info("About to FTP file: " + ZippedFilename);
                string localFilename = FTPFile(iFilename, fileName);
                logger.Info("FTP'ed file: " + ZippedFilename);

                if (localFilename.Length > 0)
                {
                    logger.Info("About to unzip file: " + ZippedFilename);
                    FullUnzippedFilename = UnzipFile(localFilename);
                    logger.Info("Unzipped file: " + FullUnzippedFilename);

                    logger.Info("About to execute stored proc sp_Importfile");
                    retCode = this.ExecuteNonQueryStoredProc("sp_Importfile", connectionName.Trim());
                    logger.Info("Executed stored proc sp_Importfile");
                }
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in ImportDataFile() - " + ErrorString);
            }
            finally
            {
                File.Delete(FullUnzippedFilename);
            }

            logger.Info("Leaving ImportDataFile(" + ZippedFilename + ")");
            return retCode;
        }

        // returns the unzipped filename on success or empty string otherwise
        private string UnzipFile(string zippedFilename)
        {
            string unzippedFilename = "";
            try
            {
                FileInfo zippedFile = new FileInfo(zippedFilename);
                unzippedFilename = zippedFilename.Remove(zippedFilename.Length - zippedFile.Extension.Length);

                FileStream zippedFileStream = zippedFile.OpenRead();
                FileStream unzippedFileStream = File.Create(unzippedFilename);
                GZipStream unzippedStream = new GZipStream(zippedFileStream, CompressionMode.Decompress);
                unzippedStream.CopyTo(unzippedFileStream);

                zippedFileStream.Close();
                unzippedFileStream.Close();

                // delete zipped file
            }
            catch (Exception ex)
            {
                unzippedFilename = "";
                ErrorString = ex.Message;
                logger.Error("Error in UnzipFile() - " + ErrorString);
            }
            finally
            {
                File.Delete(zippedFilename);
            }

            return unzippedFilename;
        }

        // returns the local file name on success or empty string otherwise
        private string FTPFile(string remoteFileName, string fileName)
        {
            string localFilename = "";
            Session session = new Session();
            try
            {
                // FTP File to local server
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = ConfigurationManager.AppSettings["HostName"],
                    UserName = ConfigurationManager.AppSettings["UserName"],
                    Password = ConfigurationManager.AppSettings["Password"],
                };
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;

                session.Open(sessionOptions);

                //string fullRemoteFilename = db.Parameters.FirstOrDefault(m=>m.Name=="RemotePath").Value + "/" + targetFolder+"/"+"cleaned"+"/"+remoteFileName;
                if (session.FileExists(remoteFileName))
                {
                    string localFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/ZipCsvs");
                    if (!Directory.Exists(localFolder))
                    {
                        Directory.CreateDirectory(localFolder);
                    }
                    localFilename = Path.Combine(localFolder, fileName);

                    logger.Info("Start FTP of " + remoteFileName);
                    session.GetFiles(remoteFileName, localFilename).Check();
                    logger.Info("Finished FTP of " + remoteFileName);
                }
                else
                    throw new Exception(remoteFileName + " doesn't exist");
            }
            catch (Exception ex)
            {
                localFilename = "";
                ErrorString = ex.Message;
                logger.Error("Error in FTPFile() - " + ErrorString);
            }
            finally
            {
                //session.Close();
            }

            return localFilename;
        }

        public override int SetDBParameters(SqlCommand myCmd)
        {
            int retCode = 1;
            ErrorString = "";
            try
            {
                SqlParameter myParm;

                myParm = myCmd.Parameters.Add("@filepath", SqlDbType.VarChar, FullUnzippedFilename.Length);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = FullUnzippedFilename;

                myParm = myCmd.Parameters.Add("@filename", SqlDbType.VarChar, ZippedFilename.Length);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = ZippedFilename;

                myParm = myCmd.Parameters.Add("@date", SqlDbType.VarChar, TargetDay.Length);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = TargetDay;
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in SetDBParameters() - " + ErrorString);
            }

            return retCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathFileInfo"></param>
        /// <returns></returns>
        public int ImportCruiseDiscoveryFile(string pathFileInfo,string ConnectionStringName)
        {
            int retCode = 0;
            try
            {
                logger.Info("Enter into ImportCruiseDiscoveryFile() of DataFileImporter class");
                FileStream stream = File.Open(pathFileInfo, FileMode.Open, FileAccess.Read);
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                DataSet result = excelReader.AsDataSet();

                if (pathFileInfo.Contains(".xlsx"))
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                }  //...
                else
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                excelReader.Close();

                DataTable dt = result.Tables[0];
                dt.Rows.RemoveAt(0);

                using (SqlConnection con = new SqlConnection(WebConfigurationManager.AppSettings[ConnectionStringName].ToString()))
                {
                    logger.Info("Establish the Sql connection");
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null))
                    {
                        CruisedDiscoveryRepository objcleanbulkdata = new CruisedDiscoveryRepository();
                        objcleanbulkdata.CleanPreviousCruiseData(ConnectionStringName);
                        logger.Info("Truncate the previous data from CruisedDiscovery table");
                        con.Open();
                        logger.Info("Open the connection of SqlBulkCopy");
                        //Set the database table name
                        sqlBulkCopy.ColumnMappings.Add(0, 1);
                        sqlBulkCopy.ColumnMappings.Add(1, 2);
                        sqlBulkCopy.ColumnMappings.Add(2, 3);
                        sqlBulkCopy.ColumnMappings.Add(3, 4);
                        sqlBulkCopy.ColumnMappings.Add(4, 5);
                        sqlBulkCopy.ColumnMappings.Add(5, 6);
                        sqlBulkCopy.ColumnMappings.Add(6, 7);
                        sqlBulkCopy.DestinationTableName = CrusieDicoveryTableName;
                        logger.Info("Copy the Data from Excel sheet to CruisedDiscovery table start");
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                        logger.Info("Copy the Data from Excel sheet to CruisedDiscovery table End");
                        logger.Info("Close the Connection for SqlBulkCopy");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
            finally
            {
           
            }
            return retCode;
        }

        public string GetDateFromString(string fileName)
        {
            string dateString = null;
            if (fileName.Contains(ConfigurationManager.AppSettings["csvFileNameFormat1"].ToString()))
            {
                dateString = fileName.Replace(ConfigurationManager.AppSettings["csvFileNameFormat1"], "").Replace(ConfigurationManager.AppSettings["Extension"], "");
            }
            else if(fileName.Contains(ConfigurationManager.AppSettings["csvFileNameFormat2"].ToString()))
            {
                dateString = fileName.Replace(ConfigurationManager.AppSettings["csvFileNameFormat2"], "").Replace(ConfigurationManager.AppSettings["Extension"], "");
            }
            else if (fileName.Contains(ConfigurationManager.AppSettings["csvFileNameFormat3"].ToString()))
            {
                dateString = fileName.Replace(ConfigurationManager.AppSettings["csvFileNameFormat3"], "").Replace(ConfigurationManager.AppSettings["Extension"], "");
            }
            DateTime dtTime = DateTime.ParseExact(dateString, "MM-dd-yyyy", CultureInfo.InvariantCulture);
            string strDate = dtTime.ToString("yyyy-MM-dd");
            return strDate;
        }
    }
}