﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.A8AdminModel
{
    public class Site
    {
        [Key]
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        // Foreign key 
        //public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public bool? AutoLogin { get; set; }
        public string ControllerIpAddress { get; set; }
        public string MySqlIpAddress { get; set; }

        [MaxLength]
        public string Term_conditions { get; set; }

        [MaxLength]
        public string TermsAndCondDoc { get; set; }
        public string RtlsUrl { get; set; }
        public string DashboardUrl { get; set; }
        public bool? IsRtls { get; set; }
        public bool? IsLocationDashboard { get; set; }
        public bool? IsCaptivePortal { get; set; }

        [MaxLength(50)]
        public string SiteIconPath { get; set; }

        //[ForeignKey("UserId")]
        //public virtual Users Users { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

      

    }
}