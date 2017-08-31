using System;
using System.Linq;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public class ApiAccessUserSessionRepository
    {
        private A8AdminDbContext db;
        private string strReturn { get; set; }


        public ApiAccessUserSessionRepository()
        {
            db = new A8AdminDbContext();
        }

        public void UpdateCreateUserSession(ApiAccessUserSession model)
        {
            try
            {
                if(UserSessionAlreadyExist(model.UserId))
                {
                    UpdateUserSession(model);
                }
                else
                {
                    AddNewUserSession(model);
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SessionId"></param>
        /// <returns></returns>

        public bool IsAuthorize(string SessionId)
        {
            bool retval;
            try
            {
                if (db.UserSession.Any(m => m.SessionId == SessionId))
                {
                    retval = true;
                }
                else
                {
                    retval = false;
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            return retval;
        }


        /// <summary>
        /// 
        /// </summary>
        public void AddNewUserSession(ApiAccessUserSession model)
        {
            try
            {
                ApiAccessUserSession objUserSession = new ApiAccessUserSession();
                objUserSession.SessionId = model.SessionId;
                objUserSession.UserId = model.UserId;
                db.UserSession.Add(objUserSession);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateUserSession(ApiAccessUserSession model)
        {
            try
            {
                var objUserSession = db.UserSession.FirstOrDefault(m => m.UserId == model.UserId);
                objUserSession.SessionId = model.SessionId;
                db.Entry(objUserSession).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UserSessionAlreadyExist(int UserId)
        {
            try
            {
                if (db.UserSession.Any(m => m.UserId == UserId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}