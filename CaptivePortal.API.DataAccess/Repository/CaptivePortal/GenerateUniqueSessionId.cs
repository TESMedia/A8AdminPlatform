using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CaptivePortal.API.Repository.CaptivePortal
{
    public static class GenerateUniqueSessionId
    {
        public static string GenerateSeesionId()
        {
            SessionIDManager manager = new SessionIDManager();
            return manager.CreateSessionID(HttpContext.Current);
        }

    }
}