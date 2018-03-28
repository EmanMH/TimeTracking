using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Business.Services
{
    public class EmployeeServices
    {
        TimeTrackingEntities _ttContext;
        public EmployeeServices()
        {
            _ttContext = new TimeTrackingEntities();
        }

        public List<serviceCode> getServiceCodes()
        {
            var svc = _ttContext.serviceCodes.ToList();
            return svc;
        }

        public TimeSheet getTimeSheetByID(int id)
        {
            return _ttContext.TimeSheets.Include(x=>x.TimeInOuts).Where(x => x.Id == id).FirstOrDefault();
        }

        public void TimeSheetdeleteById(int id)

        {
            var ts= _ttContext.TimeSheets.Where(x => x.Id == id).FirstOrDefault();
            if (ts != null)
            {
                if (ts.TimeInOuts != null)
                {
                    var lst = ts.TimeInOuts.ToList();
                    for (int i = 0; i < lst.Count; i++)
                    {
                        //  ts.TimeInOuts.Remove(ts.TimeInOuts.ToList()[i]);
                        _ttContext.TimeInOuts.Remove(lst[i]);
                    }
                }
                _ttContext.TimeSheets.Remove(ts);
                _ttContext.SaveChanges();
            }

        }


        public List<planSection> getPlans()
        {
            var plans = _ttContext.planSections.ToList();
            return plans;
        }

        public int saveItems(TimeSheet ts)
        {
                _ttContext.TimeSheets.Add(ts);
            _ttContext.SaveChanges();

            return ts.Id;

        }

        public void saveLogs(List<UserLog> uls,string empID)
        {
            var uLog = _ttContext.UserLogs.Where(x => x.empID == empID).ToList();
            if (uLog != null)
            {
                foreach (UserLog ul in uLog)
                {
                    _ttContext.UserLogs.Remove(ul);
                }
            }

            foreach (UserLog ul in uls)
                _ttContext.UserLogs.Add(ul);
            _ttContext.SaveChanges();
        }

        public List<int> getSelecedLogs(string empID)
        {
            var logID = _ttContext.UserLogs.Where(x => x.empID.Equals(empID)).Select(c => c.logItemID).ToList<int>();
            return logID;
        }

        public int getLogID(string logName)
        {
            var logID = _ttContext.LogsLkps.Where(x => x.LogName.Equals(logName)).Select(c => c.ID).FirstOrDefault();
            return logID;
        }

        public List<int> getLogsItemsIDslst(int logID, string timePart) {
            List<int> logsItemsIDslst = new List<int>();

            if (timePart.Equals("m"))
            {
                logsItemsIDslst = _ttContext.LogsItems.Where(x => x.logID == logID && x.Morning == true).Select(c => c.ID).ToList();
            }
            else if (timePart.Equals("a"))
            {
                logsItemsIDslst = _ttContext.LogsItems.Where(x => x.logID == logID && x.Afternoon == true).Select(c => c.ID).ToList();
            }
            else if (timePart.Equals("e"))
            {
                logsItemsIDslst = _ttContext.LogsItems.Where(x => x.logID == logID && x.Evening == true).Select(c => c.ID).ToList();
            }
            else
            {
                logsItemsIDslst = _ttContext.LogsItems.Where(x => x.logID == logID).Select(c => c.ID).ToList();
            }

            return logsItemsIDslst;
        }

        public TimeSheet getTimeSheet(DateTime startDate,string uname)
        {
            var timesheets = _ttContext.TimeSheets.Include(x=>x.TimeInOuts).Where(t => t.DayDate.Value == startDate &&  t.AspNetUser.UserName==uname).FirstOrDefault();
            return timesheets;
        }

        public string getUsername(string username)
        {
            var user = _ttContext.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault();
            var details = user.UserDetails.FirstOrDefault();
            return details.FirstName + " " + details.LastName;
        }

        public string getUserId(string username)
        {
            var user = _ttContext.AspNetUsers.Where(u => u.UserName == username).FirstOrDefault();

            return user.Id;
        }

        public int getServiceCodeId(string serviceCode)
        {
            var servicecode = _ttContext.serviceCodes.Where(sc => sc.Name == serviceCode).FirstOrDefault();

            return servicecode.Id;
        }

        public List<DateTime> getLast5Weeks()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime mondayOfLastWeek1 = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
            DateTime mondayOfLastWeek2 = mondayOfLastWeek1.AddDays(-(int)mondayOfLastWeek1.DayOfWeek - 6);
            DateTime mondayOfLastWeek3 = mondayOfLastWeek2.AddDays(-(int)mondayOfLastWeek2.DayOfWeek - 6);
            DateTime mondayOfLastWeek4 = mondayOfLastWeek3.AddDays(-(int)mondayOfLastWeek3.DayOfWeek - 6);
            DateTime mondayOfLastWeek5 = mondayOfLastWeek4.AddDays(-(int)mondayOfLastWeek4.DayOfWeek - 6);

            dates.Add(mondayOfLastWeek1);
            dates.Add(mondayOfLastWeek2);
            dates.Add(mondayOfLastWeek3);
            dates.Add(mondayOfLastWeek4);
            dates.Add(mondayOfLastWeek5);
            return dates;


        }

        public List<DateTime> getLast5WeeksLast()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime now = DateTime.Now.AddDays(7);
            DateTime sundayOfLastWeek1 = now.AddDays(-(int)DateTime.Now.DayOfWeek - 7);
            DateTime sundayOfLastWeek2 = sundayOfLastWeek1.AddDays(-(int)sundayOfLastWeek1.DayOfWeek-7);
            DateTime sundayOfLastWeek3 = sundayOfLastWeek2.AddDays(-(int)sundayOfLastWeek2.DayOfWeek-7);
            DateTime sundayOfLastWeek4 = sundayOfLastWeek3.AddDays(-(int)sundayOfLastWeek3.DayOfWeek-7);
            DateTime sundayOfLastWeek5 = sundayOfLastWeek4.AddDays(-(int)sundayOfLastWeek4.DayOfWeek-7);

          
            dates.Add(sundayOfLastWeek1);
            dates.Add(sundayOfLastWeek2);
            dates.Add(sundayOfLastWeek3);
            dates.Add(sundayOfLastWeek4);
            dates.Add(sundayOfLastWeek5);
            return dates;


        }
    }
}
