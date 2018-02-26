using Business.Services;
using Data;
using Save_DataToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TimeTracking.Models;
using Utilities;

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
                    tItem.dayDate= day1.ToShortDateString();
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
                    tItem.dayDate = item.dayDate.Value.Date.ToShortDateString(); ;
                    tItem.dayName = item.dayDate.Value.ToString("dddd"); //config
                    if(tsm.HasTime2 != true)
                    tsm.HasTime2 = item.TimeIn2 !=null;
                    tItem.plansectionId = item.fk_plansection.Value;
                    tItem.isAmIn = item.isInAM.Value.ToString().ToLower();
                    tItem.isAmOut = item.isOutAM.Value.ToString().ToLower();
                    tItem.serviceCodeId = item.fk_serviceCode.Value;
                    //time.serviceCodes = svc;
                    tItem.TimeInH1 = item.TimeInH1.ToString();
                    tItem.TimeInM1 = item.TimeInM1.ToString();

                    tItem.TimeOutH1 = item.TimeOutH1.ToString();
                    tItem.TimeOutM1 = item.TimeOutM1.ToString();

                    if (item.TimeIn2 != null)
                    {
                        tItem.TimeIn2H1 = item.TimeIn2H1.ToString();
                        tItem.TimeIn2M1 = item.TimeIn2M1.ToString();

                        tItem.TimeOut2H1 = item.TimeOut2H1.ToString();
                        tItem.TimeOut2M1 = item.TimeOut2M1.ToString();
                        tItem.isAmIn2 = item.isInAM2.Value.ToString().ToLower();
                        tItem.isAmOut2 = item.isOutAM2.Value.ToString().ToLower();
                        tItem.Time2 = true;
                    }
                   // tItem.serviceCodeId = item.serviceCode.Id;
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
                dateitem = new dateItem();
            }
            return Json(datestr);
        }

        [HttpPost]
        public JsonResult saveTimeSheet(string items,string backup,string livein)
        {
            //save in db
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<TimeSheetItem> tsm = jsSerializer.Deserialize<List<TimeSheetItem>>(items);

            TimeSheet ts = new Data.TimeSheet();
            ts.DayDate = DateTime.Parse(tsm[0].dayDate);
            ts.fk_statusid = 1;
            ts.fk_userId = es.getUserId(User.Identity.Name);
            ts.isBackup = backup.ToLower() == "y";
            ts.isLiveIn = livein.ToLower() == "y";
            TimeInOut tm = new TimeInOut();
            foreach (var item in tsm)
            {
                tm.dayDate= DateTime.Parse(item.dayDate);
                tm.fk_plansection = item.plansectionId;
                tm.fk_serviceCode = item.serviceCodeId;
                tm.isInAM =bool.Parse( item.isAmIn);
                tm.isOutAM =bool.Parse( item.isAmOut);
                tm.TimeInH1 = int.Parse(item.TimeInH1);
                tm.TimeInM1 = int.Parse(item.TimeInM1);

                tm.TimeOutH1 = int.Parse(item.TimeOutH1);
                tm.TimeOutM1 = int.Parse(item.TimeOutM1);

                if(item.TimeIn2H1 != "-1" && item.TimeIn2H1!=null)
                {
                    tm.isInAM2 = bool.Parse(item.isAmIn2);
                    tm.isOutAM2 = bool.Parse(item.isAmOut2);
                    tm.TimeIn2H1 = int.Parse(item.TimeIn2H1);
                    tm.TimeIn2M1 = int.Parse(item.TimeIn2M1);
                    tm.TimeOut2H1 = int.Parse(item.TimeOut2H1);
                    tm.TimeOut2M1 = int.Parse(item.TimeOut2M1);
                    ts.HasTime2 = true;

                }
                ts.TimeInOuts.Add(tm);
                tm = new TimeInOut();
            }
            es.saveItems(ts);
            //save in excel
            var svcs = es.getServiceCodes();
            ExcelTimeSheet exs = new ExcelTimeSheet();
            TimeSheetExcel tse = new TimeSheetExcel();
            TimeRecordExcel tr = new TimeRecordExcel();
            TimeExcel te = new TimeExcel();
            List<TimeRecordExcel> timesExcel = new List<TimeRecordExcel>();

            tse.EmployeeName = es.getUsername(User.Identity.Name);
            tse.FromDay = ts.DayDate.Value.ToShortDateString();
            tse.LiveInEmployee = ts.isLiveIn.Value;
            tse.ToDay = tsm[tsm.Count - 1].dayDate;
            tse.Year = "2018";

            foreach (var item in tsm)
            {
                tr.Backup = ts.isBackup.Value == true ? 'Y' : 'N';
                tr.Day= DateTime.Parse(item.dayDate).Day;
                if (item.plansectionId == 1)
                    tr.EnterPlan = "R";
                else if (item.plansectionId == 2)
                    tr.EnterPlan = "S";
                else if (item.plansectionId == 3)
                    tr.EnterPlan = "T";
                tr.MonthNumber = DateTime.Parse(item.dayDate).Month;
                tr.ServiceCode = svcs.Where(s => s.Id == item.serviceCodeId).FirstOrDefault().Name;
                te = new TimeExcel();
                te.AmOrPm = item.isAmIn=="true" ? "AM" : "PM";
                te.Hours =int.Parse( item.TimeInH1);
                te.Mins = int.Parse(item.TimeInM1);
                tr.TimeIn1 = te;
                te = new TimeExcel();
                te.AmOrPm = item.isAmOut=="true" ? "AM" : "PM";
                te.Hours = int.Parse(item.TimeOutH1);
                te.Mins = int.Parse(item.TimeOutM1);

                tr.TimeOut1 = te;
                if(item.TimeIn2H1 !="-1" && item.TimeIn2H1 !=null)
                {
                    te = new TimeExcel();
                    te.AmOrPm = item.isAmIn2=="true" ? "AM" : "PM";
                    te.Hours = int.Parse(item.TimeIn2H1);
                    te.Mins = int.Parse(item.TimeIn2M1);

                    tr.TimeIn2 = te;
                }
                if (item.TimeOut2H1 != "-1" && item.TimeOut2H1 != null)
                {
                    te = new TimeExcel();
                    te.AmOrPm = item.isAmOut2=="true" ? "AM" : "PM";
                    te.Hours = int.Parse(item.TimeOut2H1);
                    te.Mins = int.Parse(item.TimeOut2M1 );

                    tr.TimeOut2 = te;
                }
                tse.TimeRecordsLst.Add(tr);
                tr = new TimeRecordExcel();

            }
           string res= exs.FillSheet(tse,Server.MapPath(@"~/Template/Employee-Weekly-Timesheet.xls"),Server.MapPath("~/TimeSheets"));

                return Json(res);
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
