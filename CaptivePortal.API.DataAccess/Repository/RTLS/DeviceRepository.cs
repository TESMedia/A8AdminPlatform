using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.RTLS
{
    public class DeviceRepository : IDisposable
    {
        private A8AdminDbContext db = null;

        public DeviceRepository()
        {
            db = new A8AdminDbContext();
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
                    if (!(db.Device.Any(m => m.Mac == Item)))
                    {
                        Device objMac = new Device();
                        objMac.Mac = Item;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.None);
                        objMac.IsCreatedByAdmin = IsCreateFromAdmin;
                        objMac.CreatedDateTime = DateTime.Now;
                        db.Device.Add(objMac);
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
                    if (db.Device.Any(m => m.Mac == mac && m.Intstatus == intStatus))
                    {
                        var objMac = db.Device.FirstOrDefault(m => m.Mac == mac);
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                        db.Entry(objMac).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Device objMac = new Device();
                        objMac.Mac = mac;
                        objMac.Intstatus = Convert.ToInt32(DeviceStatus.Registered);
                        objMac.IsCreatedByAdmin = IsCreatedByAdmin;
                        db.Device.Add(objMac);

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
                var ObjDevice = db.Device.FirstOrDefault(m => m.Id == MacId);
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
        public void DeleteMacAddress(Device objDevice)
        {
            db.Device.Remove(objDevice);
            db.SaveChanges();
        }

        public void UpdateMacAddres(Device objDevice)
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
                    var ObjDevice = db.Device.FirstOrDefault(m => m.Mac == item);
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
            if (db.Device.FirstOrDefault(m => m.Id == MacId).Intstatus != IntStatus)
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
            var difference = lstMac.Except(db.Device.Select(m => m.Mac));
            if (difference.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Device> GetListOfMacAddress()
        {
            return db.Device.ToList();
        }

        public string[] GetMacAdressFromId(int MacId)
        {
            return new[] { db.Device.FirstOrDefault(m => m.Id == MacId).Mac };
        }

        public Device GetDeviceFromMac(string macDevice)
        {
            return db.Device.FirstOrDefault(m => m.Mac == macDevice);
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
