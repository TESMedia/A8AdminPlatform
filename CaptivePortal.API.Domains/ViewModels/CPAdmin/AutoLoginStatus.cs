﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class AutoLoginStatus
    {
        public AutoLoginStatus()
        {
            StatusReturn = new StatusReturn();
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public StatusReturn StatusReturn { get; set; }
    }
}