using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.RTLS
{
    [Serializable]
    public class StatusReturn
    {
        public int returncode { get; set; }
        public string errmsg { get; set; }
    }
}