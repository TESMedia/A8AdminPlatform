using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    public class NeighBourArea
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NeighbourAreaId { get; set; }
        public string AreaCode { get; set; }
        [ForeignKey("InterestLocation")]
        public int AreaId { get; set; }
        public virtual InterestLocation InterestLocation { get; set; }
    }
}