using CaptivePortal.API.DataAccess.Repository.CaptivePortal;
using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.ViewModels.CPAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.CPAdmin
{
    public class GroupController : Controller
    {
        Group objGroup = new Group();
        GroupRepository groupRepo = new GroupRepository();
        A8AdminDbContext db = new A8AdminDbContext();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        // GET: Group
        public ActionResult Index(int? siteId)
        {
            ViewBag.sites = from item in db.Site.ToList()
                            select new SelectListItem()
                            {
                                Value = item.SiteId.ToString(),
                                Text = item.SiteName
                            };

            GrouplistViewModel list = new GrouplistViewModel();
            list.GroupViewlist = new List<GroupViewModel>();
            var groupResult = db.Groups.Where(m => m.SiteId == siteId).ToList();
            if (groupResult.Count != 0)
            {

                var groupDetails = (from item in groupResult
                                    select new GroupViewModel()
                                    {
                                        GroupName = item.GroupName,
                                        Rule = item.Rule,
                                        GroupId = item.GroupId,
                                        NumberOfUser = db.Users.Where(m => m.GroupId == item.GroupId).ToList().Count
                                    }
                                 ).ToList();
                list.GroupViewlist.AddRange(groupDetails);
            }
            else
            {
                TempData["GroupSuc"] = "There is no group under selected site";
            }
            return View(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateGroup(Group model)
        {
            groupRepo.Create(model);
            return RedirectToAction("Index", "Group", new { siteId = model.SiteId });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteGroup(int GroupId)
        {
            groupRepo.Delete(GroupId);
            return RedirectToAction("Index", "Group");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        [HttpPost]
        public void UpdateUserGroup(UserGroup groupModel)
        {
            try
            {
                groupRepo.Update(groupModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}