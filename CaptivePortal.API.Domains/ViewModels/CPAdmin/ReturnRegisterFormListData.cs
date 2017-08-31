using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class ReturnRegisterFormListData
    {

        public ReturnRegisterFormListData()
        {
            ReteurnRegisterFormList = new List<ReturnRegisterFormData>();
        }
        public List<ReturnRegisterFormData> ReteurnRegisterFormList { get; set; }
    }

}