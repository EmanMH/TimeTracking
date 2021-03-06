﻿using Business.Services;
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
            List<TimeInOut> timeinouts = new List<TimeInOut>();
            if (timesheets !=null && timesheets.TimeInOuts!=null)
             timeinouts = timesheets.TimeInOuts.ToList();
            ModelStack mdl= new ModelStack();
            TimeSheetModel tsm = new TimeSheetModel();
            TimeSheetItem tItem = new TimeSheetItem();
            List<TimeSheetItem> timeItems = new List<TimeSheetItem>();

            var svc = Session["svc"] == null ? es.getServiceCodes() : (List<serviceCode>)Session["svc"];

            var plans = Session["plans"] == null ? es.getPlans() : (List<planSection>)Session["plans"];

            Session["svc"] = svc;
            Session["plans"] = plans;
            tsm.empName = Session["username"] == null ? es.getUsername(User.Identity.Name) : Session["username"].ToString();
            Session["username"] = tsm.empName;
            
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
                tsm.Id = 0;
                tsm.isViewOnly = false;
                tsm.isBackup = null;
                tsm.isLiveIn = null;
                tsm.startDate = day1;
                tsm.items = new List<TimeSheetItem>();
                for (int i = 0; i < 7; i++)
                {
                    tItem.Id = 0;
                    tItem.dayDate= day1.ToShortDateString();
                    tItem.dayName = day1.ToString("dddd");
                    day1=day1.AddDays(1);
                    tItem.dates = dates;
                    tItem.times = new List<times>();
                    tItem.times.Add(new times());
                    tsm.items.Add(tItem);
                    tItem = new TimeSheetItem();
                }
            }
            else
            {
                    tsm.Id = timesheets.Id;

                if (timesheets.isDraft!=null && timesheets.isDraft.Value == true)
                {
                    tsm.isViewOnly = false;
                    tsm.isDraft = true;
                }
                else
                {
                    tsm.isViewOnly = true;
                    tsm.isDraft = false;
                }
                if (timesheets.isBackup!=null)
                tsm.isBackup = timesheets.isBackup.Value;
                if (timesheets.isLiveIn != null)
                    tsm.isLiveIn = timesheets.isLiveIn.Value;
                if (timesheets.DayDate != null)
                    tsm.startDate = timesheets.DayDate.Value;
                tsm.items = new List<TimeSheetItem>();


                if (timesheets.fk_statusid == 3)
                {
                    tsm.isViewOnly = false;
                    for (int i = 0; i < 7; i++)
                    {
                        var items = timeinouts.Where(x => x.dayDate.Value.Date.ToShortDateString() == day1.ToShortDateString()).ToList();
                        var dateditems = items.GroupBy(x => x.dayDate);
                        if (items.Count > 0)
                        {
                            foreach (var itemsvals in dateditems)
                            {
                                var itemvals = itemsvals.ToList();
                                var time2 = false;
                                foreach (var item in itemvals)
                                {
                                    if(time2==false)
                                    {
                                        time2 = item.TimeIn2H1 != null;
                                    }
                                    tItem.Id = item.Id;
                                    tItem.dayDate = item.dayDate.Value.Date.ToShortDateString(); ;
                                    tItem.dayName = item.dayDate.Value.ToString("dddd"); //config
                                    if (tsm.HasTime2 != true)
                                        tsm.HasTime2 = item.TimeIn2H1 != null;
                                    times timesitem = new times();
                                    if (item.fk_plansection != null)
                                        timesitem.plansectionId = item.fk_plansection.Value;
                                    if (item.isInAM != null)
                                        timesitem.isAmIn = item.isInAM.Value.ToString().ToLower();
                                    if (item.isOutAM != null)
                                        timesitem.isAmOut = item.isOutAM.Value.ToString().ToLower();
                                    if (item.fk_serviceCode != null)
                                        timesitem.serviceCodeId = item.fk_serviceCode.Value;
                                    //time.serviceCodes = svc;
                                    if (item.TimeInH1 != null)
                                        timesitem.TimeInH1 = item.TimeInH1.ToString();
                                    if (item.TimeInM1 != null)
                                        timesitem.TimeInM1 = item.TimeInM1.ToString();
                                    if (item.TimeOutH1 != null)
                                        timesitem.TimeOutH1 = item.TimeOutH1.ToString();
                                    if (item.TimeOutM1 != null)
                                        timesitem.TimeOutM1 = item.TimeOutM1.ToString();

                                    if (item.TimeIn2H1 != null)
                                    {
                                        if (item.TimeIn2H1 != null)
                                            timesitem.TimeIn2H1 = item.TimeIn2H1.ToString();
                                        if (item.TimeIn2M1 != null)
                                            timesitem.TimeIn2M1 = item.TimeIn2M1.ToString();
                                        if (item.TimeOut2H1 != null)
                                            timesitem.TimeOut2H1 = item.TimeOut2H1.ToString();
                                        if (item.TimeOut2M1 != null)
                                            timesitem.TimeOut2M1 = item.TimeOut2M1.ToString();
                                        if (item.isInAM2 != null)
                                            timesitem.isAmIn2 = item.isInAM2.Value.ToString().ToLower();
                                        if (item.isOutAM2 != null)
                                            timesitem.isAmOut2 = item.isOutAM2.Value.ToString().ToLower();
                                        timesitem.Time2 = true;
                                        tsm.HasTime2 = true;
                                    }
                                    else
                                        timesitem.Time2 = time2;
                                    tItem.dates = dates;
                                    if (tItem.times == null)
                                        tItem.times = new List<times>();
                                    tItem.times.Add(timesitem);
                                    // tItem.serviceCodeId = item.serviceCode.Id;
                                   
                                }
                                tsm.items.Add(tItem);

                                tItem = new TimeSheetItem();
                            }

                           
                        }
                        else
                        {
                            tItem.Id = 0;
                            tItem.dayDate = day1.ToShortDateString();

                            tItem.dayName = day1.ToString("dddd");
                            tItem.dates = dates;
                            tItem.times = new List<times>();
                            tItem.times.Add(new times());

                            tsm.items.Add(tItem);
                        }
                        day1 = day1.AddDays(1);
                        tItem = new TimeSheetItem();
                    }



                }

                else
                {
                    var items = timeinouts;
                    var dateditems = items.GroupBy(x => x.dayDate);
                        foreach (var itemsvals in dateditems)
                        {
                            var itemvals = itemsvals.ToList();
                        var time2 = false;
                            foreach (var item in itemvals)
                            {
                            if (time2 == false)
                            {
                                time2 = item.TimeIn2H1 != null;
                            }
                            tItem.Id = item.Id;
                            tItem.dayDate = item.dayDate.Value.Date.ToShortDateString(); ;
                            tItem.dayName = item.dayDate.Value.ToString("dddd"); //config
                            if (tsm.HasTime2 != true)
                                tsm.HasTime2 = item.TimeIn2H1 != null;
                            times timesitem = new times();
                                if (item.fk_plansection != null)
                                    timesitem.plansectionId = item.fk_plansection.Value;
                                if (item.isInAM != null)
                                    timesitem.isAmIn = item.isInAM.Value.ToString().ToLower();
                                if (item.isOutAM != null)
                                    timesitem.isAmOut = item.isOutAM.Value.ToString().ToLower();
                                if (item.fk_serviceCode != null)
                                    timesitem.serviceCodeId = item.fk_serviceCode.Value;
                                //time.serviceCodes = svc;
                                if (item.TimeInH1 != null)
                                    timesitem.TimeInH1 = item.TimeInH1.ToString();
                                if (item.TimeInM1 != null)
                                    timesitem.TimeInM1 = item.TimeInM1.ToString();
                                if (item.TimeOutH1 != null)
                                    timesitem.TimeOutH1 = item.TimeOutH1.ToString();
                                if (item.TimeOutM1 != null)
                                    timesitem.TimeOutM1 = item.TimeOutM1.ToString();

                                if (item.TimeIn2H1 != null)
                                {
                                    if (item.TimeIn2H1 != null)
                                        timesitem.TimeIn2H1 = item.TimeIn2H1.ToString();
                                    if (item.TimeIn2M1 != null)
                                        timesitem.TimeIn2M1 = item.TimeIn2M1.ToString();
                                    if (item.TimeOut2H1 != null)
                                        timesitem.TimeOut2H1 = item.TimeOut2H1.ToString();
                                    if (item.TimeOut2M1 != null)
                                        timesitem.TimeOut2M1 = item.TimeOut2M1.ToString();
                                    if (item.isInAM2 != null)
                                        timesitem.isAmIn2 = item.isInAM2.Value.ToString().ToLower();
                                    if (item.isOutAM2 != null)
                                        timesitem.isAmOut2 = item.isOutAM2.Value.ToString().ToLower();
                                    timesitem.Time2 = true;
                                    tsm.HasTime2 = true;
                                }
                                else
                                timesitem.Time2 = time2;

                            tItem.dates = dates;
                           

                            if (tItem.times == null)
                                tItem.times = new List<times>();
                                tItem.times.Add(timesitem);
                                // tItem.serviceCodeId = item.serviceCode.Id;

                            }

                        if (time2 == true)
                        {
                            foreach (var item in tItem.times)
                            {
                                item.Time2 = true;
                            }
                        }

                        timeItems.Add(tItem);

                            tItem = new TimeSheetItem();
                        }
                    tsm.items = timeItems;
                }
            }
            mdl.model = tsm;
            mdl.modelstack = new List<TimeSheetModel>();
           // mdl.modelstack.Add(tsm);
            return Json(mdl);

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
        public JsonResult saveTimeSheet(string items, string backup, string livein, string draft, string id, string userid)
        {
            //save in db
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<TimeSheetItem> tsm = jsSerializer.Deserialize<List<TimeSheetItem>>(items);
            bool isdraft = bool.Parse(draft);
            TimeSheet ts;
            int tid=0;
            if (id != "" && id != "0")
            {
                tid = int.Parse(id);
            }
             ts = new Data.TimeSheet();
            ts.DayDate = DateTime.Parse(tsm[0].dates[0]);
            if(isdraft != true)
            ts.fk_statusid = 1;
            ts.fk_userId =userid;
            if (backup != null && backup!="")
                ts.isBackup = backup.ToLower() == "y";
            else
                ts.isBackup = null;
            if (livein != null && livein != "")
                ts.isLiveIn = livein.ToLower() == "y";
            else
            ts.isLiveIn = null;
            if (isdraft == true)
                ts.isDraft = true;
                TimeInOut tm = new TimeInOut();
            foreach (var item in tsm)
            {
                foreach (var itemtimes in item.times)
                {
                    tm.dayDate = DateTime.Parse(item.dayDate);
                    if (itemtimes.plansectionId != 0)
                        tm.fk_plansection = itemtimes.plansectionId;
                    if (itemtimes.serviceCodeId != 0)
                        tm.fk_serviceCode = itemtimes.serviceCodeId;
                    if (itemtimes.isAmIn != "-1")
                        tm.isInAM = bool.Parse(itemtimes.isAmIn);
                    if (itemtimes.isAmOut != "-1")
                        tm.isOutAM = bool.Parse(itemtimes.isAmOut);
                    if (itemtimes.TimeInH1 != "-1")
                        tm.TimeInH1 = int.Parse(itemtimes.TimeInH1);
                    if (itemtimes.TimeInM1 != "-1")
                        tm.TimeInM1 = int.Parse(itemtimes.TimeInM1);
                    if (itemtimes.TimeOutH1 != "-1")
                        tm.TimeOutH1 = int.Parse(itemtimes.TimeOutH1);
                    if (itemtimes.TimeOutM1 != "-1")
                        tm.TimeOutM1 = int.Parse(itemtimes.TimeOutM1);

                    if (itemtimes.TimeIn2H1 != "-1" && itemtimes.TimeIn2H1 != null)
                    {
                        if (itemtimes.isAmIn2 != "-1")
                            tm.isInAM2 = bool.Parse(itemtimes.isAmIn2);
                        if (itemtimes.isAmOut2 != "-1")
                            tm.isOutAM2 = bool.Parse(itemtimes.isAmOut2);
                        if (itemtimes.TimeIn2H1 != "-1")
                            tm.TimeIn2H1 = int.Parse(itemtimes.TimeIn2H1);
                        if (itemtimes.TimeIn2M1 != "-1")
                            tm.TimeIn2M1 = int.Parse(itemtimes.TimeIn2M1);
                        if (itemtimes.TimeOut2H1 != "-1")
                            tm.TimeOut2H1 = int.Parse(itemtimes.TimeOut2H1);
                        if (itemtimes.TimeOut2M1 != "-1")
                            tm.TimeOut2M1 = int.Parse(itemtimes.TimeOut2M1);
                        ts.HasTime2 = true;

                    }
                    if (isdraft != true && (tm.fk_serviceCode == 0 || tm.fk_serviceCode == null) && (tm.fk_plansection == 0 || tm.fk_plansection == null))
                        tm = new TimeInOut();
                    else
                        ts.TimeInOuts.Add(tm);
                    tm = new TimeInOut();
                }
            }
            if (isdraft != true)
                ts.isDraft = false;
             int tsid=   es.saveItems(ts);

            if(tid!=0)
            es.TimeSheetdeleteById(tid);

            if (isdraft != true)
            {
                //save in excel
                var svcs =Session["svc"]==null? es.getServiceCodes(): (List<serviceCode>)Session["svc"];
                ExcelTimeSheet exs = new ExcelTimeSheet();
                TimeSheetExcel tse = new TimeSheetExcel();
                TimeRecordExcel tr = new TimeRecordExcel();
                TimeExcel te = new TimeExcel();
                List<TimeRecordExcel> timesExcel = new List<TimeRecordExcel>();

                tse.EmployeeName = Session["username"] == null ? es.getUsername(User.Identity.Name) : Session["username"].ToString();
                tse.FromDay = tsm[0].dates[0];
                tse.LiveInEmployee = ts.isLiveIn.Value;
                tse.ToDay = tsm[0].dates[4];
                tse.Year = ts.DayDate.Value.Year.ToString();
                var itsm = ts.TimeInOuts.ToList();
                foreach (var item in itsm)
                {
                    tr.Backup = ts.isBackup.Value == true ? 'Y' : 'N';
                    tr.Day = item.dayDate.Value.Day;
                    if (item.fk_plansection == 1)
                        tr.EnterPlan = "R";
                    else if (item.fk_plansection == 2)
                        tr.EnterPlan = "S";
                    else if (item.fk_plansection == 3)
                        tr.EnterPlan = "T";

                    tr.MonthNumber = item.dayDate.Value.Month;
                    if (item.fk_serviceCode != null)
                    {
                        tr.ServiceCode = svcs.Where(s => s.Id == item.fk_serviceCode).FirstOrDefault().Name;
                        te = new TimeExcel();
                        te.AmOrPm = item.isInAM == true ? "AM" : "PM";
                        te.Hours = item.TimeInH1.Value;
                        te.Mins = item.TimeInM1.Value;
                        tr.TimeIn1 = te;
                        te = new TimeExcel();
                        te.AmOrPm = item.isOutAM == true ? "AM" : "PM";
                        te.Hours =item.TimeOutH1.Value;
                        te.Mins = item.TimeOutM1.Value;

                        tr.TimeOut1 = te;
                        if ( item.TimeIn2H1 != null)
                        {
                            te = new TimeExcel();
                            te.AmOrPm = item.isInAM2 == true ? "AM" : "PM";
                            te.Hours = item.TimeIn2H1.Value;
                            te.Mins = item.TimeIn2M1.Value;

                            tr.TimeIn2 = te;
                        }
                        if ( item.TimeOut2H1 != null)
                        {
                            te = new TimeExcel();
                            te.AmOrPm = item.isOutAM2 == true ? "AM" : "PM";
                            te.Hours = item.TimeOut2H1.Value;
                            te.Mins =item.TimeOut2M1.Value;

                            tr.TimeOut2 = te;
                        }
                        tse.TimeRecordsLst.Add(tr);
                    }
                    tr = new TimeRecordExcel();

                }
                string res = exs.FillSheet(tse, Server.MapPath(@"~/Template/Employee-Weekly-Timesheet.xlsx"), Server.MapPath("~/TimeSheets"));

                return Json(tsid);
            }
            return Json(tsid);
        }

        public ActionResult TimeSheet()
        {
            
            var empName = es.getUsername(User.Identity.Name);
            var strname = "";
            foreach (var item in empName.Split(' '))
            {
                if (strname != "")
                    strname += " ";
                if (item.Length > 1)
                    strname += item[0].ToString().ToUpper() + item.Substring(1);
                else
                    strname += item[0].ToString().ToUpper();

            }
            ViewBag.empName = strname;
            ViewBag.userid = es.getUserId(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public JsonResult saveLogs(string checkedItems, string toiletingLst)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<int> sblst = jsSerializer.Deserialize<List<int>>(checkedItems);

            List<ToiletingModel> tmlst = jsSerializer.Deserialize<List<ToiletingModel>>(toiletingLst);

            string empID = es.getUserId(User.Identity.Name); 

            List<UserLog> logs = new List<UserLog>();
           
            foreach (int i in sblst) {
                UserLog ul = new UserLog();
                ul.empID = empID;
                ul.logItemID = i;
                logs.Add(ul);
            }
           
            es.saveLogs(logs,empID);

            List<Toileting> ts = new List<Toileting>();

            foreach (ToiletingModel tm in tmlst)
            {
                foreach (ToiletingItem ti in tm.ToiletingItemLst) {
                    Toileting t = new Toileting();
                    t.EmployeeID = empID;
                    t.DayID = tm.DayID;
                    t.Urine = ti.isUrine;
                    t.UrineTime = ti.UrineTime;
                    t.Bm = ti.isBm;
                    t.BmTime = ti.BmTime;
                    ts.Add(t);
                 }
            }

            es.saveToileting(ts, empID);

            return Json("");
        }

        private void swap(List<int> lst)
        {
            int temp = 0;
            temp = lst[0];
            lst.RemoveAt(0);
            lst.Add(temp);
        }

        private List<Log> createLogLst(List<v_Logs> Logs, bool isSwapped)
        {

            List<Log> LogLst = new List<Log>();
            var LogsIems = Logs.Select(x => new { x.logLkpID, x.LogName , x.HasMorning , x.HasAfternoon, x.HasEvening }).Distinct();

            foreach (var l in LogsIems)
            {

                Log log = new Log();
                bool hasTime = false;
                log.LogName = l.LogName;
                if (l.HasMorning)
                {
                    hasTime = true;
                    log.hasMorning = true;
                    log.MorningLst = Logs.Where(x => x.logLkpID == l.logLkpID && x.Morning == true).Select(c => c.ID).ToList();
                    if (isSwapped)
                    {
                        swap(log.MorningLst);
                    }
                }

                if (l.HasAfternoon)
                {
                    hasTime = true;
                    log.hasAfternoon = true;
                    log.AfternoonLst = Logs.Where(x => x.logLkpID == l.logLkpID && x.Afternoon == true).Select(c => c.ID).ToList();
                    if (isSwapped)
                    {
                        swap(log.AfternoonLst);
                    }
                }

                if (l.HasEvening)
                {
                    hasTime = true;
                    log.hasEvening = true;
                    log.EveningLst = Logs.Where(x => x.logLkpID == l.logLkpID && x.Evening == true).Select(c => c.ID).ToList();
                    if (isSwapped)
                    {
                        swap(log.EveningLst);
                    }
                }

                if (!hasTime)
                {
                    log.hasValue = true;
                    log.ValusLst = Logs.Where(x => x.logLkpID == l.logLkpID).Select(c => c.ID).ToList();
                    if (isSwapped)
                    {
                        swap(log.ValusLst);
                    }
                }
                LogLst.Add(log);
            }

            return LogLst;
        }

        private LogItem createLogItem(List<v_Logs> Logs) {
            LogItem li = new LogItem();

            var ctgrsLst = Logs.Where(x => !string.IsNullOrEmpty(x.CategoryName)).Select(c => c.CategoryName).Distinct();

            foreach (string lg in ctgrsLst)
            {
                categorizedLogs lc = new categorizedLogs();
                List<v_Logs> lLkp = Logs.Where(x => x.CategoryName.Equals(lg)).ToList();

                lc.cateogryName = lg;
                lc.logLst = createLogLst(lLkp, false);
                li.logCtgLst.Add(lc);
            }

            li.logSwapLst = createLogLst(Logs.Where(x => x.isSwapped).ToList(), true);
            li.logsLst = createLogLst(Logs.Where(x => x.categoryID == null && !x.isSwapped).ToList(), false);

            return li;
        }

        [HttpPost]
        public JsonResult loadLogItems()
        {
            LogsModel lms = new LogsModel();

            List<v_Logs> vLogs = es.getLogs();

            string empID = es.getUserId(User.Identity.Name);

            lms.selectedLogs = es.getSelecedLogs(empID);
            
            #region Activities of Daily Living

            List<v_Logs> adLogs = vLogs.Where(x => x.logTypeName.Equals("Activities of Daily Living")).ToList<v_Logs>();
            lms.DLActivities = createLogItem(adLogs);

            #endregion

            #region Food Prep

            List<v_Logs> fpLogs = vLogs.Where(x => x.logTypeName.Equals("Food Prep")).ToList<v_Logs>();
            lms.FPActivities = createLogItem(fpLogs);
            
            #endregion

            #region Activities
            
            List<v_Logs> aLogs = vLogs.Where(x => x.logTypeName.Equals("Activities")).ToList<v_Logs>();
            lms.Activities = createLogItem(aLogs);
            
            #endregion

            #region Light Chores/House Keeping

            List<v_Logs> lchkLogs = vLogs.Where(x => x.logTypeName.Equals("Light Chores/House Keeping")).ToList<v_Logs>();
            lms.LcHkActivities = createLogItem(lchkLogs);

            #endregion

            #region Toileting

            List<Toileting> tList = es.getToileting(empID);
            List<DaysOfWeek> dLst = es.getDaysOfWeek();

            foreach(DaysOfWeek d in dLst)
            {
                ToiletingModel tm = new ToiletingModel();
                tm.DayID = d.ID;

                tm.Day = d.fullDayName;

                List<Toileting> dtList = tList.Where(x => x.DayID == d.ID).ToList<Toileting>();

                if (dtList.Capacity > 0)
                {
                    foreach (Toileting t in dtList)
                    {
                        ToiletingItem tItm = new ToiletingItem();
                        tItm.isBm = t.Bm;
                        tItm.BmTime = t.BmTime;
                        tItm.isUrine = t.Urine;
                        tItm.UrineTime = t.UrineTime;
                        tm.ToiletingItemLst.Add(tItm);
                    }
                }
                else
                {
                    ToiletingItem tItm = new ToiletingItem();
                    tItm.isBm = false;
                    tItm.BmTime = null;
                    tItm.isUrine = false;
                    tItm.UrineTime = null;
                    tm.ToiletingItemLst.Add(tItm);
                }
                lms.ToiletingLst.Add(tm);
            }

            #endregion

            return Json(lms);
        }

        public ActionResult Logs()
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
