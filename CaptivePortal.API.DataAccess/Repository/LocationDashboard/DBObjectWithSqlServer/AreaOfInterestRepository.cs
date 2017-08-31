using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Web;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
   public class AreaOfInterestRepository : DBObject
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
        public int GetAllAreaOfInterst(string connectionStringName)
        {
            int retCode = 0;
            try
            {
                retCode = this.ExecuteReaderStoredProc("Sp_GetAllArea", connectionStringName);
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
    }
}
