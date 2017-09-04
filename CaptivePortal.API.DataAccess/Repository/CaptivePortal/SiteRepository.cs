using CaptivePortal.API.Models;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class SiteRepository
    {
        private A8AdminDbContext db;
        private string compId;
        public SiteRepository()
        {
            db = new A8AdminDbContext();
        }

        public bool CreateSite(FormViewModel inputData)
        {
            try
            {
                if (inputData.CompanyName == null)
                {
                    compId = inputData.CompanyDdl;
                }
                else
                {
                    compId = db.Company.FirstOrDefault(m => m.CompanyName == inputData.CompanyName).CompanyId.ToString();
                }

                Site objSite = new Site
                {
                    SiteName = inputData.SiteName,
                    CompanyId = compId == null ? null : (int?)Convert.ToInt32(compId),
                    AutoLogin = inputData.AutoLogin,
                    ControllerIpAddress = inputData.ControllerIpAddress,
                    MySqlIpAddress = inputData.MySqlIpAddress,
                    Term_conditions = inputData.Term_conditions,
                    TermsAndCondDoc = inputData.TermsAndCondDoc,
                    DashboardUrl = inputData.DashboardUrl,
                    RtlsUrl = inputData.RtlsUrl

                };
                db.Site.Add(objSite);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool UpdateSite(FormViewModel inputData)
        {
            try
            {
                Site objSite = new Site
                {
                    SiteName = inputData.SiteName,
                    SiteId = inputData.SiteId,
                    CompanyId =Convert.ToInt32(inputData.CompanyDdl),
                    AutoLogin = inputData.AutoLogin,
                    ControllerIpAddress = inputData.ControllerIpAddress,
                    MySqlIpAddress = inputData.MySqlIpAddress,
                    Term_conditions = inputData.Term_conditions,
                    DashboardUrl = inputData.DashboardUrl,
                    RtlsUrl = inputData.RtlsUrl
                };

                db.Entry(objSite).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSiteExist(int SiteId)
        {
            return db.Site.Any(m => m.SiteId == SiteId);
        }
    }
}