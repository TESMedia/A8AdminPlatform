using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CaptivePortal.API.ViewModels.LocationDashBoard
{
    public class LocationIndicatorViewModel
    {
        public LocationIndicatorViewModel()
        {
            lstMapLocations = new List<MapLocation>();
            lstNeighBourMaps = new List<NeighBourAreaMap>();
        }
        public int AreaOfInterestId { get; set; }

        [Required(ErrorMessage ="Enter the Area Name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Enter the LocationIndicator Name")]
        public string LoctionIndicator { get; set; }
        public string NeighBourName { get; set; }

        public string SiteName { get; set; }
        public int NeighBourId { get; set; }
        public int LoctionIndicatorId { get; set; }
        public List<MapLocation> lstMapLocations { get; set; }
        public List<NeighBourAreaMap> lstNeighBourMaps { get; set; }
    }

    public class MapLocation
    {
        public string LoctionIndicator { get; set; }
        public int LoctionIndicatorId { get; set; }
        public int AreaOfInterestId { get; set; }
        public string Name { get; set; }
    }

    public class NeighBourAreaMap
    {
        public string NeighBourName { get; set; }
        public int NeighBourId { get; set; }
        public int AreaOfInterestId { get; set; }
        public string Name { get; set; }
    }
}
