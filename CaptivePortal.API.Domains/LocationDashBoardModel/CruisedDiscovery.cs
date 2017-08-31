using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    public class CruisedDiscovery
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Day { get; set; }

        public DateTime Date { get; set; }

        [StringLength(50)]
        public string CruiseName { get; set; }

        [StringLength(50)]
        public string TUIDiscovery { get; set; }

        [StringLength(50)]
        public string Arrival { get; set; }

        [StringLength(50)]
        public string Departure { get; set; }

        public int ? TimeDiff { get; set; }

    }
}