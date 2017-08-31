using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.RTLS
{
    [Serializable]
    public class LocationViewModel
    {
        public StatusReturn result;
        public List<Record> records;
        public LocationViewModel()
        {
            result = new StatusReturn();
            records = new List<Record>();
        }
    }
}