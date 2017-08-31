using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.RTLS
{
    [NotMapped]
    public class MonitorDevices
    {
        public MonitorDevices()
        {
            records = new List<MonitorDevice>();
        }
        public List<MonitorDevice> records { get; set; }
        public StatusReturn result;
    }
}