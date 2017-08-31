using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.CustomIdentity
{
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
}