using CaptivePortal.API.Models.A8AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.A8AdminPortal
{
    public class AdminSiteAccessRepository
    {

        private A8AdminDbContext db = null;
        /// <summary>
        /// 
        /// </summary>
        public AdminSiteAccessRepository()
        {
            db = new A8AdminDbContext();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<AdminSiteAccess> GetListOfAdminSiteAccess(int UserId)
        {
            return db.AdminSiteAccess.Where(m => m.UserId == UserId).ToList();
        }

    }
}
