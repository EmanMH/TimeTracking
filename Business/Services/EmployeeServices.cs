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

        public void TimeSheetdeleteById(int id)

        {
            var ts= _ttContext.TimeSheets.Where(x => x.Id == id).FirstOrDefault();
            if (ts != null)
            {
                if (ts.TimeInOuts != null)
                {
                    for (int i = 0; i < ts.TimeInOuts.Count; i++)
                    {
                        ts.TimeInOuts.Remove(ts.TimeInOuts.ToList()[i]);
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
        public void saveItems(TimeSheet ts)
        {
            _ttContext.TimeSheets.Add(ts);
            _ttContext.SaveChanges();
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
