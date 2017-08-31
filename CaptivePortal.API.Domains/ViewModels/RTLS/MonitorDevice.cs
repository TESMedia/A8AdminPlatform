using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.RTLS
{
    public class MonitorDevice
    {
        public string monitor_updated { get; set; }

        public string bn { get; set; }

        public string sn { get; set; }

        public string time_stamp { get; set; }

        public string device_id { get; set; }
    }
}