using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class CruisedDiscoveryRepository
    {
        public void CleanPreviousCruiseData(string ConnectionName)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.AppSettings[ConnectionName].ToString());
            try
            {
                SqlCommand cmd = new SqlCommand("Truncate table dbo.CruisedDiscovery", con);
                con.Open();
                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}