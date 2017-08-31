using System;
using System.Linq;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models;
using CaptivePortal.API.ViewModels.CPAdmin;
using CaptivePortal.API.Models.CaptivePortalModel;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class FormRepository
    {
        private A8AdminDbContext db = null;
        public FormRepository()
        {
            db = new A8AdminDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        public ReturnLoginFormData ReturnPageDynamicData(FormData formdata)
        {
            ReturnLoginFormData objLoginFormData = new ReturnLoginFormData();
            try
            {
                var formResult = db.Form.FirstOrDefault(m => m.SiteId == formdata.SiteId);
                objLoginFormData.SiteId = formdata.SiteId;
                objLoginFormData.BannerIcon = formResult.BannerIcon;
                objLoginFormData.BackGroundColor = formResult.BackGroundColor;
                objLoginFormData.IsPasswordRequire = formResult.IsPasswordRequire;
                objLoginFormData.LoginWindowColor = formResult.LoginWindowColor;
                objLoginFormData.LoginPageTitle = formResult.LoginPageTitle;
                objLoginFormData.ControllerIP = db.Site.FirstOrDefault(m => m.SiteId == formdata.SiteId).ControllerIpAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objLoginFormData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formdata"></param>
        /// <returns></returns>
        public ReturnRegisterFormListData ReturnRegistrationPageDynamicData(FormData formdata)
        {
            ReturnRegisterFormListData objReturnRegisterFormListData = new ReturnRegisterFormListData();
            var jsonRegisterFormData = (from item in db.FormControl.ToList()
                                        select new ReturnRegisterFormData()
                                        {
                                            ColumnName = item.LabelName,
                                            LabelNameToDisplay = item.LabelNameToDisplay,
                                            IsMandetory = item.IsMandetory,
                                        }).ToList();
            return objReturnRegisterFormListData;
        }


        /// <summary>
        /// Get the Notification string data as poer Site
        /// </summary>
        /// <returns></returns>
        public string GetTermConditionData(int SiteId)
        {
            string strReturn = null;
            try
            {
               if(IsFormExistForSite(SiteId))
                {
                    strReturn= db.Form.FirstOrDefault(m => m.SiteId == SiteId).Site.TermsAndCondDoc;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strReturn;
        }

        public Form GetFormData(int SiteId)
        {
          return db.Form.FirstOrDefault(m => m.SiteId == SiteId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool IsFormExistForSite(int SiteId)
        {
           return db.Form.Any(m => m.SiteId == SiteId);   
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public bool CreateForm(FormViewModel inputData)
        {
            try
            {
                Form objForm = new Form
                {
                    SiteId = db.Site.FirstOrDefault(m => m.SiteName == inputData.SiteName).SiteId,
                    BannerIcon = inputData.BannerIcon,
                    BackGroundColor = inputData.BackGroundColor,
                    LoginWindowColor = inputData.LoginWindowColor,
                    IsPasswordRequire = Convert.ToBoolean(inputData.IsPasswordRequire),
                    LoginPageTitle = inputData.LoginPageTitle,
                    RegistrationPageTitle = inputData.RegistrationPageTitle,
                };
                db.Form.Add(objForm);
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
        /// <param name="inputData"></param>
        /// <returns></returns>
        public bool UpdateForm(FormViewModel inputData)
        {
            try
            {
                Form objForm = new Form
                {
                    FormId = inputData.FormId,
                    SiteId = inputData.SiteId,
                    BannerIcon = inputData.BannerIcon,
                    IsPasswordRequire = Convert.ToBoolean(inputData.IsPasswordRequire),
                    BackGroundColor = inputData.BackGroundColor,
                    LoginWindowColor = inputData.LoginWindowColor,
                    LoginPageTitle = inputData.LoginPageTitle,
                    RegistrationPageTitle = inputData.RegistrationPageTitle
                };
                db.Entry(objForm).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }   
}
