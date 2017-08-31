using CaptivePortal.API.Models;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CustomIdentity;
using CaptivePortal.API.ViewModels;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.CPAdmin
{
    public class GroupController : Controller
    {
        Group objGroup = new Group();
        A8AdminDbContext db = new A8AdminDbContext();
        // GET: Group
        public ActionResult Index(string SiteId)
        {
            ViewBag.sites = from item in db.Site.ToList()
                            select new SelectListItem()
                            {
                                Value = item.SiteId.ToString(),
                                Text = item.SiteName
                            };

            GrouplistViewModel list = new GrouplistViewModel();
            list.GroupViewlist = new List<GroupViewModel>();
            var result = db.Group.ToList();

            var groupDetails = (from item in result
                                select new GroupViewModel()
                                {
                                    GroupName = item.GroupName,
                                    Rule = item.Rule,
                                    GroupId = item.GroupId,
                                    NumberOfUser= db.Users.Where(m => m.GroupId == item.GroupId).ToList().Count
                                }
                             ).ToList();
            list.GroupViewlist.AddRange(groupDetails);
            return View(list);
        }

        public ActionResult CreateGroup(Group model)
        {
            objGroup.GroupName = model.GroupName;
            objGroup.Rule = model.Rule;
            db.Group.Add(objGroup);
            db.SaveChanges();
            return RedirectToAction("Index", "Group");
        }
        [HttpPost]
        public ActionResult DeleteGroup(int GroupId)
        {
            var group = db.Group.Find(GroupId);
            db.Group.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index", "Group");
        }

        [HttpPost]
        public ActionResult UpdateUserGroup(UserGroup groupModel)
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
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", "Group",new { SiteId=2});
        }
    }
}