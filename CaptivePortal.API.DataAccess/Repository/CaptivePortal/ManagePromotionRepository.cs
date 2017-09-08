using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class ManagePromotionRepository:IDisposable
    {
         
        private A8AdminDbContext db = null;
        public ManagePromotionRepository()
        {
            db = new A8AdminDbContext();
        }
        
        public void Dispose()
        {
            db.Dispose();
        }

        public ReturnPromationalData GetManagePromotionData(int SiteId)
        {
            try
            {
                ReturnPromationalData PromotinalData = new ReturnPromationalData();
                var formResult = db.ManagePromotion.FirstOrDefault(m => m.SiteId == SiteId);
                PromotinalData.SuccessPageOption = formResult.SuccessPageOption;
                PromotinalData.WebPageURL = formResult.WebPageURL;
                PromotinalData.OptionalPictureForSuccessPage = formResult.OptionalPictureForSuccessPage;
                return PromotinalData;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Create(ManagePromotion model)
        {
            ManagePromotion objManagePromotion = new ManagePromotion();
            try
            {
                objManagePromotion.SiteId = model.SiteId;
                objManagePromotion.SuccessPageOption = model.SuccessPageOption;
                objManagePromotion.WebPageURL = model.WebPageURL;
                objManagePromotion.OptionalPictureForSuccessPage = model.OptionalPictureForSuccessPage;
                db.ManagePromotion.Add(objManagePromotion);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public bool Delete(int SiteId)
        {
            try
            {
                var pro = db.ManagePromotion.Find(SiteId);
                if (pro != null)
                {
                    db.ManagePromotion.Remove(pro);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}