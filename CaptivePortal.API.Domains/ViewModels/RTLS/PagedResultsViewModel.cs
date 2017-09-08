using CaptivePortal.API.Models.RTLSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.RTLS
{
    public class PagedResults
    {
        public PagedResults()
        {
            lstMacAddress = new List<Device>();
        }
        public int currentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public  List<Device> lstMacAddress { get; set; }
    }

}