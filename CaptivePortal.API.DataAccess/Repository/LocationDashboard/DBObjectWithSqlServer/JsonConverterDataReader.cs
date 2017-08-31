using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Repository.LocationDashBoard.DBObjectWithSqlServer
{
    public class JsonConverterDataReader
    {
        public string DataReaderToJson(SqlDataReader datareader)
        {
            string result = "";
            try
            {
                result = "[";
                int columnCount = datareader.FieldCount;
                while (datareader.Read())
                {
                    result += "{";
                    for (int x = 0; x < columnCount; x++)
                    {
                        result += "\"" + datareader.GetName(x) + "\":\"";
                        string stringValue = "";
                        if (!datareader.IsDBNull(x))
                        {
                            stringValue = datareader.GetValue(x).ToString();
                        }
                        else
                        {
                            stringValue = "NULL";
                        }
                        result += stringValue + "\"";
                        if (x < columnCount - 1) result += ",";
                    }
                    result += "},";
                }
                result = result.TrimEnd(result[result.Length - 1]) + "]";
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}