using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    public class LocationIndicator
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoctionIndicatorId { get; set; }
        [ForeignKey("InterestLocation")]
        public int AreaOfInterestId { get; set; }
        public string LoctionIndicator { get; set; }

        public virtual InterestLocation InterestLocation { get; set; }

    }
}


