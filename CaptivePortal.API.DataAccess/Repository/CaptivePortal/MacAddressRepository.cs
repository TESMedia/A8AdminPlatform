using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models;
using System.Data.Entity;
using log4net;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.ViewModels;
using CaptivePortal.API.ViewModels.CPAdmin;

namespace CaptivePortal.API.Repository.CaptivePortal
{

    public class MacAddressRepository
    {

        private int RetCode { get; set; }
        private string RetMsg { get; set; }
        ILog log = LogManager.GetLogger(typeof(UserRepository));
        private A8AdminDbContext db;
        public MacAddressRepository()
        {
            db = new A8AdminDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objMacAddress"></param>
        public void CreateNewMacDevice(MacAddress objMacAddress)
        {
            try
            {
                db.MacAddress.Add(objMacAddress);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
            }
        }

        public void RegisterMacAddressToRtls(string MacAddress)
        {
            try
            {
                var objMac = db.MacAddress.FirstOrDefault(m => m.MacAddressValue == MacAddress);
                objMac.IsRegisterInRtls = true;
                db.Entry(objMac).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }


        public void RegisterListOfMacAdressToDb(MemberDevicesModel objUserMac)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveListOfMacAddressFromDb(MemberDevicesModel objUserMac)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool IsMacAddressExistInParticularSite(string MacAddress,int SiteId)
        {
            return db.MacAddress.Any(m => m.MacAddressValue == MacAddress && m.Users.SiteId == SiteId);
        }

        public bool IsAccessToAutoLogin(string MacAddress,int SiteId)
        {
            var objMac= db.MacAddress.FirstOrDefault(m => m.MacAddressValue == MacAddress && m.Users.SiteId == SiteId);
            return (bool)objMac.Users.AutoLogin;
        }

       
        public bool IsMacAddressRegisterInRTLS(string MacAddress,int SiteId)
        {
            var objMac = db.MacAddress.FirstOrDefault(m => m.MacAddressValue == m.MacAddressValue && m.Users.SiteId == SiteId);
            return objMac.IsRegisterInRtls;
        }

        public string [] GetListMacAddress(string UserUniqueId,int SiteId)
        {
            return db.MacAddress.Where(m => m.Users.UniqueUserId == UserUniqueId && m.Users.SiteId==SiteId).Select(m => m.MacAddressValue).ToArray();
        }

    }
}