using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptivePortal.API.DataAccess
{
    public enum RepositoryResult
    {
        Success = 0,
        Failure = -1
    }
    public enum DeviceStatus
    {
        None = 0,
        Registered = 1,
        Failed = -1,
        DeRegistered = 2
    }

}
