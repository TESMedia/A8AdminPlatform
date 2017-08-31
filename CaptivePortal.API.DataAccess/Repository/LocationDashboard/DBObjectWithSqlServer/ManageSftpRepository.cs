using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Web;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class ManageSftpRepository : DBObject
    {
        private int Id { get; set; }
        public int DeleteFile(int DataFileId,string ConnectionString)
        {
            int retCode = 0;
            try
            {
                this.Id = DataFileId;
                retCode = this.ExecuteReaderStoredProc("sp_DelteLocationsDataAsPerDate", ConnectionString);
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in GetAllDateFromDB()-" + ErrorString);
            }
            return retCode;
        }

        public override int SetDBParameters(SqlCommand myCmd)
        {
            int retCode = 1;
            ErrorString = "";
            try
            {
                SqlParameter myParm;

                myParm = myCmd.Parameters.Add("@Id", SqlDbType.Int);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = Id;

            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in SetDBParameters() - " + ErrorString);
            }

            return retCode;
        }
    }
}