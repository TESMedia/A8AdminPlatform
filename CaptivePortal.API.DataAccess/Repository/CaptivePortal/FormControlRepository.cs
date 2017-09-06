using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Repository
{
    public class FormControlRepository
    {
        private int RetCode { get; set; }
        private string RetMsg { get; set; }
        private A8AdminDbContext db = null;      
        StringBuilder sb = new StringBuilder(String.Empty);

        public FormControlRepository()
        {
            db = new A8AdminDbContext();
        }
        public bool CreateFormControl(FormControlViewModel model, string IsMandetory)
        {
            try
            {
                FormControl objFormControl = new FormControl();
                objFormControl.LabelName = model.fieldlabel;
                objFormControl.LabelNameToDisplay = model.LabelNameToDisplay;
                objFormControl.IsMandetory = Convert.ToBoolean(IsMandetory);
                objFormControl.FormId = model.FormId;
                db.FormControl.Add(objFormControl);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool DeleteFormControl(int Id)
        {
            FormControl objFormControl = null;
            Form objForm = null;
            try
            {
                objFormControl = db.FormControl.FirstOrDefault(m => m.FormControlId == Id);
                objForm = db.FormControl.FirstOrDefault(m => m.FormControlId == Id).Forms;
                db.FormControl.Remove(objFormControl);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}