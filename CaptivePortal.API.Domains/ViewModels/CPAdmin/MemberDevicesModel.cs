using CaptivePortal.API.ViewModels.CPAdmins;
using System.Collections.Generic;

namespace CaptivePortal.API.ViewModels.CPAdmin
{
    public class MemberDevicesModel:MemberViewModel
    {
        public OperationType OperationType { get; set; }
        public List<MacAddesses> MacAddressList { get; set; }
    }

    public enum OperationType
    {
        Add = 1,
        Delete = 2
    }

}