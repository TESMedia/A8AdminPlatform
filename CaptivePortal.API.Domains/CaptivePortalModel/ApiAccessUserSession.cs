using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaptivePortal.API.Models.CustomIdentity;

namespace CaptivePortal.API.Models.CaptivePortalModel
{
    public class ApiAccessUserSession
    {
        [Key]
        public int UserSessionId { get; set; }
        public int UserId { get; set; }
        public string SessionId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}