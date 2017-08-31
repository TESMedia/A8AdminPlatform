using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.RTLS
{
    public class MacAddressRepository : IDisposable
    {
        private RTLSDbContext db = null;

        public MacAddressRepository()
        {
            db = new RTLSDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="Mac"></param>
        /// <returns></returns>
        public bool SaveMacAddress(string[] lstMac, bool IsCreateFromAdmin)
        {
            try
            {
                foreach (var Item in lstMac)
                {
                    if (!(db.MacAddress.Any(m => m.Mac == Item)))
                    {
                        MacAddress objMac = new MacAddress();
                        objMac.Mac = Item;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.None);
                        objMac.IsCreatedByAdmin = IsCreateFromAdmin;
                        objMac.CreatedDateTime = DateTime.Now;
                        db.MacAddress.Add(objMac);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstMac"></param>
        /// <param name="SiteName"></param>
        /// <returns></returns>
        public bool RegisterListOfMacAddresses(string[] lstMac, bool IsCreatedByAdmin)
        {
            try
            {
                foreach (var mac in lstMac)
                {
                    int intStatus = Convert.ToInt32(DeviceStatus.None);
                    //If MacAddress already exist with None status then 
                    if (db.MacAddress.Any(m => m.Mac == mac && m.Intstatus == intStatus))
                    {
                        var objMac = db.MacAddress.FirstOrDefault(m => m.Mac == mac);
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                        db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        MacAddress objMac = new MacAddress();
                        objMac.Mac = mac;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                        objMac.IsCreatedByAdmin = IsCreatedByAdmin;
                        db.MacAddress.Add(objMac);

                    }
                    db.SaveChanges();
                }
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
        /// <param name="MacId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public bool UpdateStatusToRegister(int MacId)
        {
            try
            {
                var ObjDevice = db.MacAddress.FirstOrDefault(m => m.Id == MacId);
                ObjDevice.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
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
        /// <param name="Mac"></param>
        public void DeleteMacAddress(MacAddress objDevice)
        {
            db.MacAddress.Remove(objDevice);
            db.SaveChanges();
        }

        public void UpdateMacAddres(MacAddress objDevice)
        {
            db.Entry(objDevice).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MacAddresses"></param>
        /// <returns></returns>
        public bool DeRegisterListOfMacs(String[] MacAddresses)
        {
            try
            {
                foreach (var item in MacAddresses)
                {
                    var ObjDevice = db.MacAddress.FirstOrDefault(m => m.Mac == item);
                    ObjDevice.Intstatus = Convert.ToInt32(DeviceStatus.DeRegistered);
                    db.Entry(ObjDevice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public bool CheckMacAddressExitOrNot(int MacId, int IntStatus)
        {
            if (db.MacAddress.FirstOrDefault(m => m.Id == MacId).Intstatus != IntStatus)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckListExistOrNot(string[] lstMac)
        {
            var difference = lstMac.Except(db.MacAddress.Select(m => m.Mac));
            if (difference.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<MacAddress> GetListOfMacAddress()
        {
            return db.MacAddress.ToList();
        }

        public string[] GetMacAdressFromId(int MacId)
        {
            return new[] { db.MacAddress.FirstOrDefault(m => m.Id == MacId).Mac };
        }

        public MacAddress GetDeviceFromMac(string macDevice)
        {
            return db.MacAddress.FirstOrDefault(m => m.Mac == macDevice);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
