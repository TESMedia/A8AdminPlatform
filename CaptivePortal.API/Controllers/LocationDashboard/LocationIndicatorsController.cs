using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaptivePortal.API.Controllers.LocationDashboard
{
    public class LocationIndicatorsController : Controller
    {
        // GET: LocationIndicator
        public ActionResult CreateLocation()
        {
            return View();
        }

        public ActionResult EditLocation()
        {
            return View();
        }

        public ActionResult LocationDashboard()
        {
            return View();
        }
        public ActionResult LocationsMapping()
        {
            return View();
        }
        public ActionResult UploadFile()
        {
            return View();
        }
    }
}