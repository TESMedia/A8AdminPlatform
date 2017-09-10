using CaptivePortal.API.Models.LocationDashBoardModel;
using CaptivePortal.API.ViewModels.LocationDashBoard;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace a8Dashboard.APIControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("locationIndicators/api")]
    public class LocationIndicatorsApiController : ApiController
    {
        [HttpGet]
        [Route("Index")]
        public List<LocationIndicatorViewModel> Index(string SiteName)
        {
            List<LocationIndicatorViewModel> lstLocationIndicatorViewModel = new List<LocationIndicatorViewModel>();
            try
            {

                using (var db = new LocationDashBoardDbContext(SiteName.Trim()))
                {
                    var interLocationsData = db.InterestLocation.ToList();

                    foreach (var item in interLocationsData.GroupBy(m => new { m.AreaOfInterestId, m.Name }))
                    {
                        LocationIndicatorViewModel objLocationIndicatorViewModel = new LocationIndicatorViewModel();
                        objLocationIndicatorViewModel.AreaOfInterestId = item.Key.AreaOfInterestId;
                        objLocationIndicatorViewModel.Name = item.Key.Name;

                        foreach (var item1 in db.LocationIndicator.Where(m => m.AreaOfInterestId == objLocationIndicatorViewModel.AreaOfInterestId))
                        {
                            objLocationIndicatorViewModel.LoctionIndicator += item1.LoctionIndicator + ",";
                        }
                        foreach (var item2 in db.NeighBourAreas.Where(m => m.AreaId == objLocationIndicatorViewModel.AreaOfInterestId))
                        {
                            objLocationIndicatorViewModel.NeighBourName += item2.AreaCode + ",";
                        }
                        lstLocationIndicatorViewModel.Add(objLocationIndicatorViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstLocationIndicatorViewModel;


        }

        [HttpGet]
        [Route("Details")]
        public HttpResponseMessage Details(int? id, string SiteName)
        {
            LocationIndicator locationIndicator = null;
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(SiteName))
            {
                locationIndicator = db.LocationIndicator.Find(id);
                if (locationIndicator == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return Request.CreateResponse(locationIndicator);
        }

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Create(LocationIndicatorViewModel objlocationIndicator)
        {

            InterestLocation locationModel = new InterestLocation();

            using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(objlocationIndicator.SiteName))
            {

                if (!string.IsNullOrEmpty(objlocationIndicator.Name))
                {
                    locationModel.Name = objlocationIndicator.Name;
                    db.InterestLocation.Add(locationModel);
                    db.SaveChanges();
                }

                var location = db.InterestLocation.Where(a => a.Name == locationModel.Name).SingleOrDefault();
                if (!string.IsNullOrEmpty(objlocationIndicator.LoctionIndicator))
                {
                    LocationIndicator indicatorModel = new LocationIndicator();
                    indicatorModel.LoctionIndicator = objlocationIndicator.LoctionIndicator;
                    indicatorModel.AreaOfInterestId = location.AreaOfInterestId;
                    db.LocationIndicator.Add(indicatorModel);
                }

                if (!string.IsNullOrEmpty(objlocationIndicator.NeighBourName))
                {

                    NeighBourArea objNeighBourArea = new NeighBourArea();
                    objNeighBourArea.AreaId = location.AreaOfInterestId;
                    objNeighBourArea.AreaCode = objlocationIndicator.NeighBourName;
                    db.NeighBourAreas.Add(objNeighBourArea);
                }

                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("Edit")]
        public HttpResponseMessage Edit(int id, string SiteName)
        {
            List<LocationIndicatorViewModel> lstLocationIndicatorViewModel = new List<LocationIndicatorViewModel>();
            LocationIndicatorViewModel objLocationIndicatorViewModel = new LocationIndicatorViewModel();
            objLocationIndicatorViewModel.SiteName = SiteName;
            objLocationIndicatorViewModel.AreaOfInterestId = id;
            using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(SiteName.Trim()))
            {
                objLocationIndicatorViewModel.Name = db.InterestLocation.First(m => m.AreaOfInterestId == id).Name;
                foreach (var item in db.LocationIndicator.Where(m => m.AreaOfInterestId == id))
                {
                    MapLocation objMapLocation = new MapLocation();
                    objMapLocation.LoctionIndicatorId = item.LoctionIndicatorId;
                    objMapLocation.LoctionIndicator = item.LoctionIndicator;
                    objMapLocation.AreaOfInterestId = item.AreaOfInterestId;
                    objLocationIndicatorViewModel.lstMapLocations.Add(objMapLocation);
                }

                foreach (var item in db.NeighBourAreas.Where(m => m.AreaId == id))
                {
                    NeighBourAreaMap objNeighBourMapLocation = new NeighBourAreaMap();
                    objNeighBourMapLocation.NeighBourId = item.NeighbourAreaId;
                    objNeighBourMapLocation.NeighBourName = item.AreaCode.ToString();
                    objNeighBourMapLocation.AreaOfInterestId = item.AreaId;
                    objLocationIndicatorViewModel.lstNeighBourMaps.Add(objNeighBourMapLocation);
                }
            }


            lstLocationIndicatorViewModel.Add(objLocationIndicatorViewModel);
            return Request.CreateResponse(objLocationIndicatorViewModel);
        }

        [HttpPost]
        [Route("Edit")]
        public HttpResponseMessage Edit(LocationIndicatorViewModel locationIndicator)
        {
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(locationIndicator.SiteName.Trim()))
                {
                    var objInterestLoction = db.InterestLocation.Where(p => p.AreaOfInterestId == locationIndicator.AreaOfInterestId).FirstOrDefault();
                    objInterestLoction.Name = locationIndicator.Name;
                    db.Entry(objInterestLoction).State = EntityState.Modified;
                    foreach (var Item in locationIndicator.lstMapLocations)
                    {
                        var objlocationIndicator = new LocationIndicator();
                        if (db.LocationIndicator.Any(m => m.LoctionIndicatorId == Item.LoctionIndicatorId))
                        {
                            objlocationIndicator = db.LocationIndicator.Where(m => m.AreaOfInterestId == locationIndicator.AreaOfInterestId && m.LoctionIndicatorId == Item.LoctionIndicatorId).FirstOrDefault();
                            objlocationIndicator.LoctionIndicator = Item.LoctionIndicator;
                            db.Entry(objlocationIndicator).State = EntityState.Modified;
                        }
                        else
                        {
                            objlocationIndicator.LoctionIndicator = Item.LoctionIndicator;
                            objlocationIndicator.AreaOfInterestId = Item.AreaOfInterestId;
                            db.LocationIndicator.Add(objlocationIndicator);
                        }
                    }

                    foreach (var item in locationIndicator.lstNeighBourMaps)
                    {
                        var ObjNeighBourArea = new NeighBourArea();
                        if (db.NeighBourAreas.Any(m => m.NeighbourAreaId == item.NeighBourId))
                        {
                            ObjNeighBourArea = db.NeighBourAreas.Where(m => m.NeighbourAreaId == item.NeighBourId && m.AreaId == locationIndicator.AreaOfInterestId).FirstOrDefault();
                            ObjNeighBourArea.AreaCode = item.NeighBourName;
                            db.Entry(ObjNeighBourArea).State = EntityState.Modified;
                        }
                        else
                        {
                            ObjNeighBourArea.AreaCode = item.NeighBourName;
                            ObjNeighBourArea.AreaId = item.AreaOfInterestId;
                            db.NeighBourAreas.Add(ObjNeighBourArea);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPost]
        [Route("Delete")]
        public HttpResponseMessage Delete(LocationIndicatorViewModel objLocation)
        {
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(objLocation.SiteName))
                {
                    var objAreaOfInterest = db.InterestLocation.Where(a => a.AreaOfInterestId == objLocation.AreaOfInterestId).FirstOrDefault();
                    db.InterestLocation.Remove(objAreaOfInterest);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPost]
        [Route("DeleteLocationIndicator")]
        public HttpResponseMessage DeleteLocationIndicator(LocationIndicatorViewModel objLocation)
        {
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(objLocation.SiteName))
                {
                    LocationIndicator objLocationIndicator = db.LocationIndicator.Where(b => b.LoctionIndicatorId == objLocation.LoctionIndicatorId).FirstOrDefault();
                    db.LocationIndicator.Remove(objLocationIndicator);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("DeleteNeighBourArea")]
        public HttpResponseMessage DeleteNeighBourArea(LocationIndicatorViewModel objLocation)
        {
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(objLocation.SiteName))
                {
                    NeighBourArea objNeighBours = db.NeighBourAreas.Where(b => b.NeighbourAreaId == objLocation.NeighBourId).FirstOrDefault();
                    db.NeighBourAreas.Remove(objNeighBours);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
