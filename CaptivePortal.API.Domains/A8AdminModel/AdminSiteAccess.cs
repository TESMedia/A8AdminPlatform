using CaptivePortal.API.Models.CustomIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.A8AdminModel
{
    public class AdminSiteAccess
    {
        [Key]
        public int AdminSiteAccessId { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }
}