using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CaptivePortal.API.Models.CaptivePortalModel
{
    public class FormControl
    {
        [Key]
        public int FormControlId { get; set; }
        //Foreign key
        public int FormId { get; set; }
        public string LabelName { get; set; }
        public string LabelNameToDisplay { get; set; }
        public bool IsMandetory { get; set; }

        [ForeignKey("FormId")]
        public virtual Form Forms { get; set; }
    }
}