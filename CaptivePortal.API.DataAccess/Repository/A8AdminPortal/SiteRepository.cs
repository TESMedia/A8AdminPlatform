using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public Site GetSiteAsPerId(int SiteId)
        {
            return db.Site.FirstOrDefault(m => m.SiteId == SiteId);
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
                    RtlsUrl = inputData.RtlsUrl,
                    SiteIconPath=inputData.SiteImagePath
                };
                db.Site.Add(objSite);
                db.SaveChanges();
                inputData.SiteId = objSite.SiteId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool UpdateSite(FormViewModel inputData)
        {
            int compId = 0;
            try
            {
                if (inputData.CompanyId != 0)
                {
                    compId = inputData.CompanyId;
                }
                else
                {
                    compId = Convert.ToInt32(inputData.CompanyDdl);
                }
                var objSite = db.Site.Find(inputData.SiteId);

                objSite.SiteName = inputData.SiteName;
                objSite.CompanyId = compId;
                objSite.AutoLogin = inputData.AutoLogin;
                objSite.ControllerIpAddress = inputData.ControllerIpAddress;
                objSite.MySqlIpAddress = inputData.MySqlIpAddress;
                objSite.Term_conditions = inputData.Term_conditions;
                objSite.DashboardUrl = inputData.DashboardUrl;
                objSite.SiteIconPath = inputData.SiteImagePath;
                objSite.RtlsUrl = inputData.RtlsUrl;
               

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public List<Site> GetListOfSiteGroupPerSiteId(int SiteId)
        {
            return db.Site.Where(m => m.CompanyId == db.Site.Where(n => n.SiteId == SiteId).FirstOrDefault().CompanyId).ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public List<Site> GetListOfSiteGroupPerSiteName(string SiteName)
        {
            return db.Site.Where(m => m.CompanyId == db.Site.Where(n => n.SiteName == SiteName).FirstOrDefault().CompanyId).ToList();
        }

    }
}