using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.LocationDashBoard
{
    public class ReportViewModel
    {
        public string Day = "";
        public string FromHour = "";
        public string ToHour = "";
        public string Location = "";
        public string DwellTime = "";
        //public string DeltaTime = "";
        public string ConnectionName;
    }
}