using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.A8AdminModel
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Rule { get; set; }

        public int? SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

    }
}