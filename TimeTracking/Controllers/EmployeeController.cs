using Business.Services;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        EmployeeServices es = new EmployeeServices();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public class dateItem
        {
            public int Id { get; set; }
            public string  date { get; set; }

        }


        [HttpPost]
        public JsonResult getTimeSheet(string dateSelected)
        {
            string str = " to ";
            string[] dates = dateSelected.Split(str.ToCharArray());
            var day1 = DateTime.Parse(dates[0]);
            var timesheets = es.getTimeSheet(DateTime.Parse(dates[0]), User.Identity.Name);
            TimeSheetModel tsm = new TimeSheetModel();
            TimeSheetItem tItem = new TimeSheetItem();
            List<TimeSheetItem> timeItems = new List<TimeSheetItem>();
            var svc = es.getServiceCodes();
            var plans = es.getPlans();
            tsm.empName = es.getUsername(User.Identity.Name);
            svc srv = new Models.svc();
            tsm.serviceCodes = new List<Models.svc>();
            tsm.PlanSections = new List<plan>();
            foreach (var item in svc)
            {
                srv.Id = item.Id;
                srv.Name = item.Name;
                tsm.serviceCodes.Add(srv);
                srv = new Models.svc();
            }
            plan pln = new Models.plan();
            foreach (var item in plans)
            {
                pln.Id = item.Id;
                pln.Name = item.Name;
                tsm.PlanSections.Add(pln);
                pln = new Models.plan();
            }
            if (timesheets==null)
            {
                tsm.isViewOnly = false;
                tsm.isBackup = false;
                tsm.isLiveIn = false;
                tsm.startDate = day1;
                tsm.items = new List<TimeSheetItem>();
                for (int i = 0; i < 7; i++)
                {
                    tItem.dayDate= day1;
                    tItem.dayName = day1.ToString("dddd");
                    day1=day1.AddDays(1);
                    tsm.items.Add(tItem);
                    tItem = new TimeSheetItem();
                }
            }
            else
            {
                tsm.isViewOnly = true;
                tsm.isBackup = timesheets.isBackup.Value;
                tsm.isLiveIn = timesheets.isLiveIn.Value;
                tsm.startDate = timesheets.DayDate.Value;
                tsm.items = new List<TimeSheetItem>();

                foreach (var item in timesheets.TimeInOuts)
                {
                    tItem.dayDate = item.dayDate.Value.Date;
                    tItem.dayName = item.dayDate.Value.ToString("dddd"); //config
                    if(tsm.HasTime2 != true)
                    tsm.HasTime2 = item.TimeIn2 !=null;
                    tItem.plansectionId = item.planSection.Id;
                    tItem.isAmIn = item.isInAM.Value;
                    tItem.isAmOut = item.isOutAM.Value;
                    tItem.serviceCodeId = item.serviceCode.Id;
                    //time.serviceCodes = svc;
                    tItem.TimeIn = item.TimeIn;
                    tItem.TimeOut = item.TimeOut; //format
                    tItem.TimeIn2 = item.TimeIn2;
                    tItem.TimeOut2 = item.TimeOut2;
                    tItem.isAmIn2 = item.isInAM2.Value;
                    tItem.isAmOut2 = item.isOutAM2.Value;
                    tItem.serviceCodeId = item.serviceCode.Id;
                    timeItems.Add(tItem);
                    tItem = new TimeSheetItem();
                }
                tsm.items = timeItems;
            }
            return Json(tsm);

        }

        [HttpPost]
        public JsonResult loadDates()
        {
            var dates = es.getLast5Weeks();
            var dates2 = es.getLast5WeeksLast();

            List<dateItem> datestr = new List<dateItem>();
            var dateitem = new dateItem();
            int i = 0;
            foreach (var item in dates)
            {
                dateitem.date = item.ToShortDateString() + " to " + dates2[i].ToShortDateString();
                dateitem.Id = i + 1;
                i++;
                datestr.Add(dateitem);
            }
            return Json(datestr);
        }

        [HttpPost]
        public JsonResult saveTimeSheet(string items)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            TimeSheetModel tsm= jsSerializer.Deserialize<TimeSheetModel>(items);
            //save in db
            //save in excel
            return null;
        }

        public ActionResult TimeSheet()
        {
            
            ViewBag.empName = es.getUsername(User.Identity.Name);


            return View();
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
