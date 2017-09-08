using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CaptivePortal.API.Models.RTLSModel
{
    public class Device
    {
        public int Id { get; set; }

        public string Mac { get; set; }

        public int Intstatus { get; set; }

        public bool IsCreatedByAdmin { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public bool IsAdminSelected { get; set; }

        public bool IsRegisterInRtls { get; set; }

        public bool IsTracking { get; set; }

        public bool IsDisplay { get; set; }
    }

}