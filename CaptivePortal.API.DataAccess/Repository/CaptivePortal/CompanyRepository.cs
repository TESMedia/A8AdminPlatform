using CaptivePortal.API.Models;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class CompanyRepository
    {
        private int RetCode { get; set; }
        private string RetMsg { get; set; }
        int orgId = 0;
        int compId = 0;

        private A8AdminDbContext db;

        public CompanyRepository()
        {
            db = new A8AdminDbContext();
        }
        public void CreateCompany(FormViewModel model)
        {
            if (model.CompanyName != null)
            {
                Company objCompany = new Company();
                objCompany.CompanyName = model.CompanyName;
                objCompany.OrganisationId = orgId == 0 ? null : (int?)Convert.ToInt32(orgId);
                db.Company.Add(objCompany);
                db.SaveChanges();
                compId = objCompany.CompanyId;
            }
            
           
        }
    }
}