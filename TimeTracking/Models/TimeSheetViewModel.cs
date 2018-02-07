using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{
    public class TimeSheetViewModel
    {
        [Required]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

       
    }
}