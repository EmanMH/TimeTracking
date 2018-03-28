using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{

    public class LogsModel
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string ServiceCode { get; set; }
        public List<int> selectedLogs = new List<int>();
        public LogItem DLActivities = new LogItem();
        public LogItem FPActivities = new LogItem();
        public LogItem Activities = new LogItem();
        public LogItem LcHkActivities = new LogItem();
    }

    public class Log
    {
        public string LogName { get; set; }
        public bool hasMorning { get; set; }
        public bool hasAfternoon { get; set; }
        public bool hasEvening { get; set; }
        public bool hasValue { get; set; }
        public List<int> MorningLst = new List<int>();
        public List<int> AfternoonLst = new List<int>();
        public List<int> EveningLst = new List<int>();
        public List<int> ValusLst = new List<int>();
    }

    public class categorizedLogs
    {
        public string cateogryName { get; set; }
        public List<Log> logLst = new List<Log>();
    }

    public class LogItem
    {
        public List<Log> logSwapLst = new List<Log>();
        public List<categorizedLogs> logCtgLst = new List<categorizedLogs>();
        public List<Log> logsLst = new List<Log>();
    }

}