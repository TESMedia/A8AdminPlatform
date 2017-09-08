using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.RTLS
{
    public class AppLogRepository : IDisposable
    {

        private A8AdminDbContext db;
        public AppLogRepository()
        {
            db = new A8AdminDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable GetAppLogs(jQueryDataTableParamModel param)
        {
            var allLocationData = db.AppLogs.ToList();
            IEnumerable<AppLog> filteredLocationData;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredLocationData = allLocationData.ToList().Where(c => c.Date == Convert.ToDateTime(param.sSearch));
            }
            else
            {
                filteredLocationData = allLocationData;
            }

            var displayLocationData = filteredLocationData;
            var result = from c in displayLocationData
                         select new { Id = c.Id, Date = c.Date.ToShortDateString(), Thread = c.Thread, Level = c.Level, Logger = c.Logger, Message = c.Message, Exception = c.Exception };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CountAppLogs()
        {
            return db.AppLogs.Count();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
