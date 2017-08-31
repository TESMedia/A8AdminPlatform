using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class StatusReturn
    {
        public int ReturnCode { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }

        public StatusReturn()
        {

        }
        public StatusReturn(int retcode,string strMsg,string type)
        {
            ReturnCode = retcode;
            Message = strMsg;
            Type = type;
        }
    }
}