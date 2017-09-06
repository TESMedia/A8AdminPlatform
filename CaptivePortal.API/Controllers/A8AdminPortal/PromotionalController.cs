using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Repository.CaptivePortal;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.A8AdminPortal
{
    public class PromotionalController : Controller
    {
        A8AdminDbContext db = new A8AdminDbContext();
        ManagePromotionRepository managePromotionRepo = new ManagePromotionRepository();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        /// GET:Create promotional page.
        public ActionResult Create(int? SiteId)
        {
            if (SiteId == null)
            {
                TempData["SiteIdCheck"] = "Please select any of the site and then manage promotional thing or If site is not there create new site";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// POST:Save promotional material for splash page after login or registration submit.
        [HttpPost]
        public ActionResult Save(ManagePromotion model)
        {
            string optionalPicturePath = null;
            string OptionalPictureForSuccessPage = null;
            ManagePromotion objManagePromotion = new ManagePromotion();
            var promo = db.ManagePromotion.ToList();
            if (promo.Count != 0)
            {
                var pro = db.ManagePromotion.Where(m => m.SiteId == model.SiteId).FirstOrDefault();
                if (pro != null)
                {
                    managePromotionRepo.Delete(model.SiteId);

                }
                //image path
                if (Request.Files["OptionalPicture"].ContentLength > 0)
                {
                    var httpPostedFile = Request.Files["OptionalPicture"];
                    string savedPath = HostingEnvironment.MapPath("/Images/" + model.SiteId);
                    optionalPicturePath = "/Images/" + model.SiteId + "/" + httpPostedFile.FileName;
                    string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                    if (!System.IO.Directory.Exists(savedPath))
                    {
                        Directory.CreateDirectory(savedPath);
                    }
                    httpPostedFile.SaveAs(completePath);
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    OptionalPictureForSuccessPage = baseUrl + optionalPicturePath;
                    model.OptionalPictureForSuccessPage = OptionalPictureForSuccessPage;
                }

                managePromotionRepo.Create(model);

            }
            else
            {
                //image path
                if (Request.Files["OptionalPicture"].ContentLength > 0)
                {
                    var httpPostedFile = Request.Files["OptionalPicture"];
                    string savedPath = HostingEnvironment.MapPath("/Images/" + model.SiteId);
                    optionalPicturePath = "/Images/" + model.SiteId + "/" + httpPostedFile.FileName;
                    string completePath = System.IO.Path.Combine(savedPath, httpPostedFile.FileName);

                    if (!System.IO.Directory.Exists(savedPath))
                    {
                        Directory.CreateDirectory(savedPath);
                    }
                    httpPostedFile.SaveAs(completePath);
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    model.OptionalPictureForSuccessPage = OptionalPictureForSuccessPage;
                    OptionalPictureForSuccessPage = baseUrl + optionalPicturePath;
                    model.OptionalPictureForSuccessPage = OptionalPictureForSuccessPage;
                }

                managePromotionRepo.Create(model);

            }
            return RedirectToAction("Index", "Home");
        }
        
    }
}