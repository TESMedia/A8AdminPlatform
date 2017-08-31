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

        private A8AdminDbContext db;

        public CompanyRepository()
        {
            db = new A8AdminDbContext();
        }


        public void CreateCompany(FormViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}