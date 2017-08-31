using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaptivePortal.API.Models.CustomIdentity;

namespace CaptivePortal.API.Models.CaptivePortalModel
{
    public class MacAddress
    {
        [Key()]
        public int MacId { get; set; }

        [MaxLength(20)]
        public string MacAddressValue { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Users { get; set; }

        public string BrowserName { get; set; }

        public string OperatingSystem { get; set; }

        public bool IsMobile { get; set; }

        public string UserAgentName { get; set; }

        public bool IsRegisterInRtls { get; set; }

    }

    
}