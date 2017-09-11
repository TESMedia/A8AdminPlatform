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
            try
            {
                
                if (model.CompanyName != null)
                {
                    Company objCompany = new Company();
                    objCompany.CompanyName = model.CompanyName;
                    objCompany.CompanyIcon = model.CompanyIcon;
                    objCompany.OrganisationId = model.OrganisationId==0  ? null : (int?)Convert.ToInt32(model.OrganisationId);
                    db.Company.Add(objCompany);
                    db.SaveChanges();
                    model.CompanyId = objCompany.CompanyId;
                }
                else
                {
                    model.CompanyId = Convert.ToInt32(model.CompanyDdl);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCompany(FormViewModel model)
        {
            try
            {
                if(model.CompanyId!=0 && model.OrganisationId!=0)
                {
                    compId = model.CompanyId;
                    orgId = model.OrganisationId;
                }
                else
                {
                    compId = Convert.ToInt32(model.CompanyDdl);
                    orgId = Convert.ToInt32(model.organisationDdl);
                }
                var objCompany = db.Company.Find(compId);
                objCompany.CompanyName = db.Company.FirstOrDefault(m => m.CompanyId == compId).CompanyName;
                objCompany.CompanyIcon = model.CompanyIcon;
                objCompany.OrganisationId = model.OrganisationId == 0 ? null : (int?)Convert.ToInt32(model.OrganisationId); 
                db.Entry(objCompany).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}