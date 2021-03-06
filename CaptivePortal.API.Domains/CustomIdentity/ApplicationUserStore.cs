﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CaptivePortal.API.Models.A8AdminModel;
using System.Data.Entity;

namespace CaptivePortal.API.Models.CustomIdentity
{
    public class ApplicationUserStore :
     UserStore<ApplicationUser, ApplicationRole, int,
     ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserStore<ApplicationUser, int>, IDisposable
    {
        public ApplicationUserStore()
            : base(new A8AdminDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }

        public override Task CreateAsync(ApplicationUser user)
        {
            return base.CreateAsync(user);
        }
    }
}