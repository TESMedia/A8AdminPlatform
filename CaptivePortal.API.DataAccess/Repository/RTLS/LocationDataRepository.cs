using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.RTLS
{
    public class LocationDataRepository : IDisposable
    {
        private A8AdminDbContext db;
        public LocationDataRepository()
        {
            db = new A8AdminDbContext();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<LocationData> GetListOfLocationData(jQueryDataTableParamModel param)
        {
            IEnumerable<LocationData> allLocationData = null;
            int TotalRecords = db.LocationData.Count();
            if (!string.IsNullOrEmpty(param.MacAddress))
            {
                allLocationData = db.LocationData.ToList();
            }
            else
            {
                allLocationData = db.LocationData.Where(m => m.mac == param.MacAddress);
            }
            allLocationData = allLocationData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                allLocationData = allLocationData.Where(c => c.AreaName.Contains(param.sSearch) || c.bn.Contains(param.sSearch) || c.mac.Contains(param.sSearch));
            }

            return allLocationData;
        }


        public int CountLocationsData(string DeviceId)
        {
            return db.LocationData.Where(m => m.mac == DeviceId).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CountLocationsData()
        {
            return db.LocationData.Count();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
