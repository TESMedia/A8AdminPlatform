using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTLS.ViewModel
{
    public class RequestLocationDataVM
    {
        public RequestLocationDataVM()
        {
            Devices = new List<Device>();
            RtlsConfiguration = new RtlsConfiguration();
        }
        public string[] MacAddresses { get; set; }

        public string Mac { get; set; }

        public bool IsDisplay { get; set; }

        public bool IscreatedByAdmin { get; set; }

        public List<Device> Devices { get; set; }

        public RtlsConfiguration RtlsConfiguration { get; set; }
    }
}