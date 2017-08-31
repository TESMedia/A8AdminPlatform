using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.A8AdminModel
{
    public class Organisation
    {
        [Key]
        public int ? OrganisationId { get; set; }
        public string OrganisationName { get; set; }
    }
}