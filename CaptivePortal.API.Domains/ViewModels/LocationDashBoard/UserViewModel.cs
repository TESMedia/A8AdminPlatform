using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaptivePortal.API.ViewModels.LocationDashBoard
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Profile = new List<SelectListItem>();
        }

        [Display(Name = "Id")]
        public string UserId { get; set; }



        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }



        public List<SelectListItem> Profile { get; set; }


        [Display(Name = "Status")]
        public string UserStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/mmm/yyyy}")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/mmm/yyyy}")]
        public DateTime LastLoginDate { get; set; }
        public Status UserStatuses { get; set; }
        public string Role { get; set; }
        public enum Status
        {

            Successed = 20,
            Pending = 10,
            Declined = 30

        };
    }
}