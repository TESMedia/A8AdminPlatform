﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class GeneratingHtmlViewModel
    {
        public int SiteId { get; set; }
        public string RegisterUrl { get; set; }
        public string LoginUrl { get; set; }
    }
}