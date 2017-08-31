using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using CaptivePortal.API.ViewModels.LocationDashBoard;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class ReportRepository : DBObject
    {
        public int Location;
        public DateTime TargetDay = DateTime.Now;
        public string ShipStartDatetime;
        public string ShipEndDatetime;
        public string Day = "";
        public string FromHour; // from 0 to 23
        public string ToHour;  // from 1 to 24
        public int DwellTime;
        public int NewVisits;
        public int RepeatVisits;
        public int Connections;
        public int PassersBy;
        public int VisitBounseForTwoMin;
        public int VisitBounseForFiveMin;
        public double AverageDwellTime;
        public double AverageFrequency;
        public string BusiestVisitHour;
        public string BusiestVisitPassersBy;
        public string CruiseName;
        public int WindowConversion;

        public ReportRepository()
        {

        }

        public string GetJSON()
        {
            string JSONString = "{" +
                                    "\"NoOfNewVisits\":\"" + NewVisits + "\"," +
                                    "\"NoOfReturns\":\"" + RepeatVisits + "\"," +
                                    "\"NoOfConnection\":\"" + Connections + "\"," +
                                    "\"NoOfPassersBy\":\"" + PassersBy + "\"," +
                                    "\"AvgDwelltime\":\"" + AverageDwellTime + "\"," +
                                    "\"AvgFrequency\":\"" + AverageFrequency + "\"," +
                                    "\"VisitBounseForTwoMin\":\"" + VisitBounseForTwoMin + "\"," +
                                    "\"VisitBounseForFiveMin\":\"" + VisitBounseForFiveMin + "\"," +
                                    "\"BusiestVisitHour\":\"" + BusiestVisitHour + "\"," +
                                    "\"BusiestVisitPassersBy\":\"" + BusiestVisitPassersBy + "\"," +
                                    "\"CruiseName\":\"" + CruiseName + "\"," +
                                    "\"WindowConversion\":\"" + WindowConversion + "\""+
                                "}";
            return JSONString;
        }

        public int GenerateReport(ReportViewModel objReportParms)
        {
            int retCode = 0;
            ErrorString = "";

            try
            {
                string DayAsString = objReportParms.Day;
                TargetDay = DateTime.Parse(DayAsString);
                ToHour = objReportParms.ToHour.Trim();
                FromHour = objReportParms.FromHour.Trim();
                ShipStartDatetime = TargetDay.Add(TimeSpan.Parse(FromHour)).ToString("yyyy-MM-dd HH:mm:ss");
                ShipEndDatetime = TargetDay.Add(TimeSpan.Parse(ToHour)).ToString("yyyy-MM-dd HH:mm:ss");
                DwellTime = Convert.ToInt32(objReportParms.DwellTime.Trim());
                Location = Convert.ToInt32(objReportParms.Location);
                retCode = this.ExecuteReaderStoredProc("sp_GetNumberOfVisit", objReportParms.ConnectionName);
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in GenerateReport() - " + ErrorString);
            }

            return retCode;
        }

        public override int PopulateFromDB(SqlDataReader myReader, ref int startIndex, bool read)
        {
            int retCode = 0;
            ErrorString = "";

            try
            {
                if (read)
                    myReader.Read();
                Connections = myReader.GetInt32(startIndex++);
                NewVisits = myReader.GetInt32(startIndex++);
                PassersBy = myReader.GetInt32(startIndex++);
                RepeatVisits = myReader.GetInt32(startIndex++);
                AverageDwellTime = Convert.ToDouble(myReader.GetDecimal(startIndex++));
                AverageFrequency = Convert.ToDouble(myReader.GetDecimal(startIndex++));
                VisitBounseForTwoMin = myReader.GetInt32(startIndex++);
                VisitBounseForFiveMin = myReader.GetInt32(startIndex++);
                BusiestVisitHour = myReader.GetString(startIndex++);
                BusiestVisitPassersBy = myReader.GetString(startIndex++);
                CruiseName = myReader.GetString(startIndex++);
                WindowConversion = myReader.GetInt32(startIndex++);
            }
            catch (Exception ex)
            {
                retCode = -1;
                ErrorString = ex.Message;
                logger.Error("Error in PopulateFromDB() - " + ex.Message);
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

                myParm = myCmd.Parameters.Add("@Area", SqlDbType.Int);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = Location;

                myParm = myCmd.Parameters.Add("@ShipStartDateTime", SqlDbType.DateTime);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = ShipStartDatetime;

                myParm = myCmd.Parameters.Add("@ShipLastDateTime", SqlDbType.DateTime);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = ShipEndDatetime;

                myParm = myCmd.Parameters.Add("@DwellTime", SqlDbType.Int);
                myParm.Direction = ParameterDirection.Input;
                myParm.Value = DwellTime;

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