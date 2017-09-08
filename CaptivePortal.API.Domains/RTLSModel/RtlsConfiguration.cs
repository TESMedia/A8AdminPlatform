using CaptivePortal.API.Models.A8AdminModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.Models.RTLSModel
{
    public class RtlsConfiguration
    {
        public int Id { get; set; }

        public string FattiEngineSiteName { get; set; }
    
        public string FattiEngineBuildingName { get; set; }

        public int ? SiteId { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Sites { get; set; }
    }
}
