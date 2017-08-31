using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;

namespace CaptivePortal.API.Models.LocationDashBoardModel
{
    /// <summary>
    /// Model for Storing all the Data from SFTP Server
    /// </summary>
    public class DataFile
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime DateOfFile { get; set; }
        public bool IsInSftp { get; set; }

        [NotMapped]
        public string TargetDate { get; set; }

        /// <summary>
        /// Method to get all the File List from the Database
        /// </summary>
        /// <param name="connectionString">The SiteName as per ConnectionString</param>
        /// <returns></returns>
        public string GetAllFileNames(string connectionString)
        {
            string retString = "";
            try
            {
                using (LocationDashBoardDbContext db = new LocationDashBoardDbContext(connectionString))
                {
                    var fileListInSftp = db.DataFiles.OrderBy(m => m.DateOfFile).Where(m => m.IsInSftp == true).ToList();
                    var fileListInDB = db.DataFiles.OrderBy(m => m.DateOfFile).Where(m => m.IsInSftp == false).ToList();
                    var objects = new { FilesInSftp = fileListInSftp, FilesInDb = fileListInDB };
                    retString = new JavaScriptSerializer().Serialize(objects);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retString;
        }
    }
}