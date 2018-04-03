using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{
    public class SheetReviewmodel
    {
        public List<timeSheet> Pending { get; set; }
        public List<timeSheet> Approved { get; set; }
        public List<timeSheet> Rejected { get; set; }

    }

    public class timeSheet
    {

    }
}