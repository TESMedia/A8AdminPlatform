using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    public class InterestLocation
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AreaOfInterestId { get; set; }
        public string Name { get; set; }

        //public string GetAllAreaOfInterst(string ConnectionString)
        //{
        //    string returnString = "";
        //    using (ApplicationDbContext db = new ApplicationDbContext(ConnectionString))
        //    {
        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        try
        //        {
        //            List<InterestLocation> lstAreaOfInterst = db.InterestLocation.ToList();
        //            returnString = js.Serialize(lstAreaOfInterst);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return returnString;
        //}
    }
}

