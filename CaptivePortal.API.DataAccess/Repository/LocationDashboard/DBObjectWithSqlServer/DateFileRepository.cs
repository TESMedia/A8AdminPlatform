using CaptivePortal.API.Models.LocationDashBoardModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class DateFileRepository : DBObject
    {
        public string result;

        public string GetJSON()
        {
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetAllDateFromDB(string connectionStringName)
        {
            int retCode = 0;
            try
            {
                retCode = this.ExecuteReaderStoredProc("sp_GetDate", connectionStringName);
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in GetAllDateFromDB()-" + ErrorString);
            }
            return retCode;
        }



        public override int PopulateFromDB(SqlDataReader myReader, ref int startIndex, bool read)
        {
            int retCode = 0;
            ErrorString = "";
            try
            {
                JsonConverterDataReader objConverter = new JsonConverterDataReader();
                result = objConverter.DataReaderToJson(myReader);
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in PopulateFromDB() - " + ex.Message);
            }

            return retCode;
        }


        /// <summary>
        /// Method to get all the File List from the Database
        /// </summary>
        /// <param name="connectionString">The SiteName as per ConnectionString</param>
        /// <returns></returns>
        public string GetAllFileNames(string connectionString)
        {
            string retString = "";
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(connectionString))
                {
                    var fileListInSftp = db.DataFiles.OrderBy(m => m.DateOfFile).Where(m => m.IsInSftp == true).ToList();
                    var fileListInDB = db.DataFiles.OrderBy(m => m.DateOfFile).Where(m => m.IsInSftp == false).ToList();
                    var objects = new { FilesInSftp = fileListInSftp, FilesInDb = fileListInDB };
                    retString = new JavaScriptSerializer().Serialize(objects);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retString;
        }
    }
}