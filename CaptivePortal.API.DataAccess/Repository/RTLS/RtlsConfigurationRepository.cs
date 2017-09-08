using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.RTLSModel;
using RTLS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.RTLS
{
    public class RtlsConfigurationRepository : IDisposable
    {
        A8AdminDbContext db = null;
        public RtlsConfigurationRepository()
        {
            db = new A8AdminDbContext();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void AddUpdateRtlsConfiguration(RtlsConfiguration model)
        {
            if (!db.RtlsConfigurations.Any(m=>m.SiteId==model.SiteId))
            {
                db.RtlsConfigurations.Add(model);
            }
            else
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteName"></param>
        /// <returns></returns>
        public RtlsConfiguration GetRtlsConfigurationAsPerSite(int SiteId)
        {
            if (db.RtlsConfigurations.Any(m=>m.SiteId== SiteId))
            {
                return db.RtlsConfigurations.FirstOrDefault(m => m.SiteId == SiteId);
            }
            else
            {
                return new RtlsConfiguration();
            }
          
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public string GetSiteName(int SiteId)
        {
            return db.Site.FirstOrDefault(m => m.SiteId == SiteId).SiteName;
        }
    }
}
