﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{
    public class TimeSheetModel
    {
        public int Id { get; set; }
        public string empName { get; set; }
        public bool? isBackup { get; set; }
        public bool? isLiveIn { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool isViewOnly { get; set; }
        public List<TimeSheetItem> items { get; set; }
        public List<plan> PlanSections { get; set; }
        public List<svc> serviceCodes { get; set; }
        public bool HasTime2 { get; set; }
        public bool isDraft { get; set; }
        public bool isChangedA { get; set; }
        public bool ChangeCountA { get; set; }
    }

    public class TimeSheetItem
    {
        public int Id { get; set; }
        public string[] dates { get; set; }
        public string dayName { get; set; }
        public string dayDate { get; set; }
        public int plansectionId { get; set; }
        public string Plan { get; set; }
        public string TimeInH1 { get; set; }
        public string TimeInH2 { get; set; }
        public string TimeInM1 { get; set; }
        public string TimeInM2 { get; set; }

        public string TimeOutH1 { get; set; }
        public string TimeOutH2 { get; set; }
        public string TimeOutM1 { get; set; }
        public string TimeOutM2 { get; set; }

        public string isAmIn { get; set; }
        public string isAmOut { get; set; }
        public string TimeIn2H1 { get; set; }
        public string TimeIn2H2 { get; set; }
        public string TimeIn2M1 { get; set; }
        public string TimeIn2M2 { get; set; }

        public string TimeOut2H1 { get; set; }
        public string TimeOut2H2 { get; set; }
        public string TimeOut2M1 { get; set; }
        public string TimeOut2M2 { get; set; }

        public string isAmIn2 { get; set; }
        public string isAmOut2 { get; set; }
        public int serviceCodeId { get; set; }
        public bool Time2 { get; set; }

        public int ccplansectionId { get; set; }
        public int ccserviceCodeId { get; set; }
        public int ccisAmIn2 { get; set; }
        public int ccisAmOut2 { get; set; }
        public int ccTimeOut2H1  { get; set; }
        public int ccTimeOut2M1  { get; set; }
        public int ccTimeIn2H1 { get; set; }
        public int ccTimeIn2M1 { get; set; }
        public int ccisAmIn  { get; set; }
        public int ccisAmOut { get; set; }
        public int ccTimeOutH1 { get; set; }
        public int ccTimeOutM1 { get; set; }
        public int ccTimeInH1 { get; set; }
        public int ccTimeInM1 { get; set; }




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