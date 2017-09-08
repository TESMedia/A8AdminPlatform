using CaptivePortal.API.Models.A8AdminModel;
using CaptivePortal.API.Models.CaptivePortalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class ReturnMacDevices
    {
        public ReturnMacDevices()
        {
            MacAddressList = new List<MacAddress>();
            objReturn = new StatusReturn();
        }

        public List<MacAddress> MacAddressList { get; set; }
        public StatusReturn objReturn {get;set;}
    }
}