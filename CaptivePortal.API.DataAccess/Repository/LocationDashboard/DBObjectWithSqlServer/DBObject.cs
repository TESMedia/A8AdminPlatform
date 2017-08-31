using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class DBObject
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DBObject()
        {
        }

        public string ErrorString = "";

        public virtual int PopulateFromDB(SqlDataReader myReader, ref int startIndex, bool read)
        {
            return 1;
        }

        public virtual int PopulateHeaderFromDB(SqlDataReader myReader, ref int startIndex, bool read)
        {
            return 1;
        }

        public virtual int SetDBParameters(SqlCommand myCmd)
        {
            return 1;
        }

        public int ExecuteReaderWithHeaderStoredProc(string storedProcName,string connectionName)
        {
            ErrorString = "";
            SqlConnection myConn = null;
            int retCode = 1;

            try
            {
                string myDBConnectionString = WebConfigurationManager.AppSettings[connectionName].ToString();
                myConn = new SqlConnection(myDBConnectionString);

                SqlCommand myCmd = new SqlCommand(storedProcName, myConn);
                myCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter myParm;
                myParm = myCmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                myParm.Direction = ParameterDirection.ReturnValue;

                if (SetDBParameters(myCmd) == -1)
                    throw new Exception(ErrorString);

                myConn.Open();
                SqlDataReader myReader = myCmd.ExecuteReader();
                int startIndex = 0;
                if (PopulateHeaderFromDB(myReader, ref startIndex, true) == -1)
                    throw new Exception(ErrorString);

                if (myReader.NextResult())
                {
                    startIndex = 0;
                    if (PopulateFromDB(myReader, ref startIndex, true) == -1)
                        throw new Exception(ErrorString);
                }

                //What is the return value?
                myReader.Close();

                retCode = Int32.Parse(myCmd.Parameters["RETURN_VALUE"].Value.ToString());
            }
            catch (Exception ex)
            {
                ErrorString = "Error running " + storedProcName + ": " + ex.Message;
                logger.Error(ErrorString);
                retCode = -1;
            }
            finally
            {
                if (myConn != null)
                    myConn.Close();
            }
            return retCode;
        }

        public int ExecuteReaderStoredProc(string storedProcName,string connectionName)
        {
            ErrorString = "";
            SqlConnection myConn = null;
            int retCode = 1;

            try
            {
                string myDBConnectionString = WebConfigurationManager.AppSettings[connectionName].ToString();
                myConn = new SqlConnection(myDBConnectionString);

                SqlCommand myCmd = new SqlCommand(storedProcName, myConn);
                myCmd.CommandType = CommandType.StoredProcedure;
                myCmd.CommandTimeout = 1000;

                SqlParameter myParm;
                myParm = myCmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                myParm.Direction = ParameterDirection.ReturnValue;

                if (SetDBParameters(myCmd) == -1)
                    throw new Exception(ErrorString);

                myConn.Open();
                SqlDataReader myReader = myCmd.ExecuteReader();
                int startIndex = 0;
                if (PopulateFromDB(myReader, ref startIndex, true) == -1)
                    throw new Exception(ErrorString);

                //What is the return value?
                myReader.Close();

                retCode = Int32.Parse(myCmd.Parameters["RETURN_VALUE"].Value.ToString());
            }
            catch (Exception ex)
            {
                ErrorString = "Error running " + storedProcName + ": " + ex.Message;
                logger.Error(ErrorString);
                retCode = -1;
            }
            finally
            {
                if (myConn != null)
                    myConn.Close();
            }
            return retCode;
        }

        public int ExecuteReaderHeaderStoredProc(string storedProcName, string connectionName)
        {
            ErrorString = "";
            SqlConnection myConn = null;
            int retCode = 1;

            try
            {
                string myDBConnectionString = WebConfigurationManager.AppSettings[connectionName].ToString();
                myConn = new SqlConnection(myDBConnectionString);

                SqlCommand myCmd = new SqlCommand(storedProcName, myConn);
                myCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter myParm;
                myParm = myCmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                myParm.Direction = ParameterDirection.ReturnValue;

                if (SetDBParameters(myCmd) == -1)
                    throw new Exception(ErrorString);

                myConn.Open();
                SqlDataReader myReader = myCmd.ExecuteReader();
                int startIndex = 0;
                if (PopulateHeaderFromDB(myReader, ref startIndex, true) == -1)
                    throw new Exception(ErrorString);

                //What is the return value?
                myReader.Close();

                retCode = Int32.Parse(myCmd.Parameters["RETURN_VALUE"].Value.ToString());
            }
            catch (Exception ex)
            {
                ErrorString = "Error running " + storedProcName + ": " + ex.Message;
                logger.Error(ErrorString);
                retCode = -1;
            }
            finally
            {
                if (myConn != null)
                    myConn.Close();
            }
            return retCode;
        }

        public int ExecuteNonQueryStoredProc(string storedProcName, SqlConnection dbConn, SqlTransaction dbTran)
        {
            int retCode = 1;
            ErrorString = "";

            try
            {
                SqlCommand myCmd = new SqlCommand(storedProcName, dbConn);
                if (dbTran != null)
                    myCmd.Transaction = dbTran;
                myCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter myParm;
                myParm = myCmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                myParm.Direction = ParameterDirection.ReturnValue;

                if (SetDBParameters(myCmd) == -1)
                    throw new Exception(ErrorString);

                myCmd.CommandTimeout = 1000;
                myCmd.ExecuteNonQuery();

                retCode = Int32.Parse(myCmd.Parameters["RETURN_VALUE"].Value.ToString());

                if (retCode == -1)
                    throw new Exception("Error running stored proc '" + storedProcName + "'");
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error(ErrorString);
            }

            return retCode;
        }

        public int ExecuteNonQueryStoredProc(string storedProcName,string connectionName)
        {
            ErrorString = "";
            int retCode = 1;
            SqlConnection myConn = null;

            try
            {
                string myDBConnectionString = WebConfigurationManager.AppSettings[connectionName].ToString();
                myConn = new SqlConnection(myDBConnectionString);
                myConn.Open();

                retCode = ExecuteNonQueryStoredProc(storedProcName, myConn, null);
            }
            catch (Exception e)
            {
                ErrorString = e.Message;
                logger.Error(ErrorString);
                retCode = -1;
            }
            finally
            {
                if (myConn != null)
                    myConn.Close();
            }

            return retCode;
        }
    }
}
