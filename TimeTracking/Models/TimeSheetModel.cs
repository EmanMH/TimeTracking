using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{
    public class TimeSheetModel
    {
        public string empName { get; set; }
        public bool isBackup { get; set; }
        public bool isLiveIn { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool isViewOnly { get; set; }
        public List<TimeSheetItem> items { get; set; }
        public List<plan> PlanSections { get; set; }
        public List<svc> serviceCodes { get; set; }
        public bool HasTime2 { get; set; }


    }

    public class TimeSheetItem
    {
        public string dayName { get; set; }
        public string dayDate { get; set; }
        public int plansectionId { get; set; }
        public string Plan { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string isAmIn { get; set; }
        public string isAmOut { get; set; }
        public string TimeIn2 { get; set; }
        public string TimeOut2 { get; set; }
        public string isAmIn2 { get; set; }
        public string isAmOut2 { get; set; }
        public int serviceCodeId { get; set; }
        public bool Time2 { get; set; }


    }

    //public class Times
    //{
    //    public string TimeIn { get; set; }
    //    public string TimeOut { get; set; }
    //    public bool isAmIn { get; set; }
    //    public bool isAmOut { get; set; }
    //    public int serviceCodeId { get; set; }
    //}
    public class svc
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}