using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess.Repository.CaptivePortal
{
    public class GroupRepository
    {
        A8AdminDbContext db = new A8AdminDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Create(Group model)
        {
            try
            {
                Group objGroup = new Group();
                objGroup.GroupName = model.GroupName;
                objGroup.Rule = model.Rule;
                db.Groups.Add(objGroup);
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
        /// <param name="GroupId"></param>
        public void Delete(int GroupId)
        {
            try
            {
                var group = db.Groups.Find(GroupId);
                db.Groups.Remove(group);
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
        /// <param name="groupModel"></param>
        public void Update(UserGroup groupModel)
        {
            try
            {
                ApplicationUser user = new ApplicationUser();
                for (int i = 0; i < groupModel.UserIdList.Count; i++)
                {
                    user = db.Users.Find(groupModel.UserIdList[i].UserId);
                    user.GroupId = groupModel.GroupId;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
