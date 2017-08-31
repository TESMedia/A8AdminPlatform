using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaptivePortal.API.Models;
using CaptivePortal.API.Models.CaptivePortalModel;
using CaptivePortal.API.Models.A8AdminModel;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class UserMacAddressDetails
    {
        public Models.CustomIdentity.ApplicationUser objUser { get; set; }
        public MacAddress objMacAddress { get; set; }
        public UsersAddress objAddress { get; set; }
    }
}