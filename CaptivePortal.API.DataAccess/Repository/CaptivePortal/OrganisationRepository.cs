using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class OrganisationRepository
    {
        private int RetCode { get; set; }
        private string RetMsg { get; set; }
        private A8AdminDbContext db = null;

        public OrganisationRepository()
        {
            db = new A8AdminDbContext();
        }

        public bool CreateOrganisation(FormViewModel inputData)
        {
            try
            {
                int orgId = inputData.organisationDdl;

                if (inputData.OrganisationName != null)
                {
                    Organisation objOrganisation = new Organisation
                    {
                        OrganisationName = inputData.OrganisationName
                    };
                    db.Organisation.Add(objOrganisation);
                    db.SaveChanges();
                    orgId = Convert.ToInt32(objOrganisation.OrganisationId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}