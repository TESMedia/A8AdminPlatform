using System;
using CaptivePortal.API.Models.A8AdminModel;


namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class UserAddressRepository
    {
        private A8AdminDbContext db;
        public UserAddressRepository()
        {
            db = new A8AdminDbContext();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAddress"></param>
        public void CreateUserAddress(UsersAddress objAddress)
        {
            try
            {
                db.UsersAddress.Add(objAddress);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
            }
        }
    }
}