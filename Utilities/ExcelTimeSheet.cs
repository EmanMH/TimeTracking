using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;

namespace Save_DataToExcel
{
    public class TimeExcel
    {
        public int Hours { get; set; }
        public int Mins { get; set; }
        public string AmOrPm { get; set; }
    }

    public class TimeRecordExcel
    {
        public int MonthNumber { get; set; }
        public int Day { get; set; }
        public string ServiceCode { get; set; }
        public string EnterPlan { get; set; }
        public char Backup { get; set; }
        public TimeExcel TimeIn1 = new TimeExcel();
        public TimeExcel TimeOut1 = new TimeExcel();
        public TimeExcel TimeIn2 = new TimeExcel();
        public TimeExcel TimeOut2 = new TimeExcel();
        public TimeSpan TotalWorkedHours { get; set; }

    }

    public class TimeSheetExcel
    {
        public string EmployeeName { get; set; }
        public string EmployeeID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Year { get; set; }
        public string FromDay { get; set; }
        public string ToDay { get; set; }
        public bool LiveInEmployee { get; set; }
        public TimeSpan Total032WorkedHours { get; set; }
        public TimeSpan Total011WorkedHours { get; set; }
        public List<TimeRecordExcel> TimeRecordsLst = new List<TimeRecordExcel>();

    }

    public class ExcelTimeSheet
    {
        public CultureInfo oldCI { get; set; }
        public string[] StringtoStringArray(string s)
        {
            string[] strArr = new string[7];
            char[] cr = s.ToCharArray();

            for (int i = 0; i < cr.Length; i++)
            {
                strArr[i] = cr[i].ToString();
            }
            return strArr;
        }

        public TimeSpan CalculateTotalWorkingHours(int tih, int tim, string ti_am_pm, int toh, int tom, string to_am_pm) {
            var ci = new CultureInfo("en-US");

            DateTime entry = DateTime.ParseExact(tih + ":" + tim + " " + ti_am_pm, "h:m tt", ci);
            DateTime leave = DateTime.ParseExact(toh + ":" + tom + " " + to_am_pm, "h:m tt", ci);
           
            return leave - entry;

            //string startTime = tih.ToString()+":"+tim.ToString()+" "+ti_am_pm;
            //string endTime = toh.ToString() + ":" + tom.ToString() + " " + to_am_pm;

            //TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
            //return duration;
        }

        void SetNewCurrentCulture()
        {
            oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        void ResetCurrentCulture()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }

        public string FillSheet(TimeSheetExcel ts,string templatepath,string timesheetpath)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(templatepath))
                {
                    int RowNum = 8;
                    string fileName = "";
                    pck.Load(stream);
                    var ws = pck.Workbook.Worksheets.First();
                    ws.Cells["C2:N2"].Value = ts.EmployeeName;
                    ws.Cells["C5:F5"].Value = ts.Year;
                    ws.Cells["L5:S5"].Value = ts.FromDay;
                    ws.Cells["Z5:AF5"].Value = ts.ToDay;
                    if (ts.LiveInEmployee)
                        ws.Cells["S28"].Value = "X";
                    else
                        ws.Cells["W28"].Value = "X";
                    double cnt = (double)ts.TimeRecordsLst.Count / (double)15;
                    int count = 0;
                    int sheetindex = 0;
                    if ((cnt % 1) > 0)
                    {
                        count = (int)cnt;
                        count++;
                    }
                    else
                    {
                        count = (int)cnt;
                    }


                    for (int i = 1; i < count; i++)
                    {
                        pck.Workbook.Worksheets.Add("Sheet ("+i.ToString()+")",ws);
                    }

                    foreach (TimeRecordExcel tr in ts.TimeRecordsLst)
                    {
                        ws.Cells["A" + RowNum.ToString()].Value = tr.MonthNumber.ToString("00");
                        ws.Cells["B" + RowNum.ToString()].Value = tr.Day.ToString("00");
                        ws.Cells["C" + RowNum.ToString()].Value = tr.ServiceCode;
                        ws.Cells["D" + RowNum.ToString()+":F" + RowNum.ToString()].Value = tr.EnterPlan.ToUpper();
                        ws.Cells["G" + RowNum.ToString()].Value = tr.Backup.ToString().ToUpper();
                        ws.Cells["H" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn1.Hours.ToString("00"))[0];
                        ws.Cells["I" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn1.Hours.ToString("00"))[1];
                        ws.Cells["J" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn1.Mins.ToString("00"))[0];
                        ws.Cells["K" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn1.Mins.ToString("00"))[1];
                        ws.Cells["L" + RowNum.ToString()+":M" + RowNum.ToString()].Value= tr.TimeIn1.AmOrPm.ToUpper();
                        ws.Cells["N" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut1.Hours.ToString("00"))[0];
                        ws.Cells["O" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut1.Hours.ToString("00"))[1];
                        ws.Cells["P" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut1.Mins.ToString("00"))[0];
                        ws.Cells["Q" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut1.Mins.ToString("00"))[1];
                        ws.Cells["R" + RowNum.ToString()+":S" + RowNum.ToString()].Value = tr.TimeOut1.AmOrPm.ToUpper();

                        var calculatedHours = CalculateTotalWorkingHours(tr.TimeIn1.Hours, tr.TimeIn1.Mins, tr.TimeIn1.AmOrPm
                                                                           , tr.TimeOut1.Hours, tr.TimeOut1.Mins, tr.TimeOut1.AmOrPm);
                        if (calculatedHours.TotalHours < 0)
                            tr.TotalWorkedHours +=TimeSpan.FromHours(24)+ calculatedHours;
                        else
                            tr.TotalWorkedHours += calculatedHours;


                        if (tr.TimeIn2.Hours != 0)
                        {
                             ws.Cells["T" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn2.Hours.ToString("00"))[0];
                            ws.Cells["U" + RowNum.ToString() ].Value = StringtoStringArray(tr.TimeIn2.Hours.ToString("00"))[1];
                            ws.Cells["V" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn2.Mins.ToString("00"))[0];
                            ws.Cells["W" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeIn2.Mins.ToString("00"))[1];
                            ws.Cells["X" + RowNum.ToString()+":Y" + RowNum.ToString()].Value = tr.TimeIn2.AmOrPm.ToUpper();
                             ws.Cells["Z" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut2.Hours.ToString("00"))[0];
                            ws.Cells["AA" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut2.Hours.ToString("00"))[1];
                            ws.Cells["AB" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut2.Mins.ToString("00"))[0];
                            ws.Cells["AC" + RowNum.ToString()].Value = StringtoStringArray(tr.TimeOut2.Mins.ToString("00"))[1];
                            ws.Cells["AD" + RowNum.ToString()+":AE" + RowNum.ToString()].Value = tr.TimeOut2.AmOrPm.ToUpper();

                             calculatedHours= CalculateTotalWorkingHours(tr.TimeIn2.Hours, tr.TimeIn2.Mins, tr.TimeIn2.AmOrPm
                                                                           , tr.TimeOut2.Hours, tr.TimeOut2.Mins, tr.TimeOut2.AmOrPm) ;
                            if (calculatedHours.TotalHours < 0)
                                tr.TotalWorkedHours  += TimeSpan.FromHours(24) + calculatedHours;
                            else
                                tr.TotalWorkedHours += calculatedHours;

                            if (tr.ServiceCode.Equals("032"))
                            {
                                if(calculatedHours.TotalHours <= 0)
                                ts.Total032WorkedHours += TimeSpan.FromHours(24) + calculatedHours;
                                else
                                    ts.Total032WorkedHours += calculatedHours;

                            }
                            else if (tr.ServiceCode.Equals("011"))
                            {
                                if (calculatedHours.TotalHours <= 0)
                                    ts.Total011WorkedHours += TimeSpan.FromHours(24) + calculatedHours;
                                else
                                    ts.Total011WorkedHours += calculatedHours;
                            }

                        }
                         calculatedHours= CalculateTotalWorkingHours(tr.TimeIn1.Hours, tr.TimeIn1.Mins, tr.TimeIn1.AmOrPm
                                                                           , tr.TimeOut1.Hours, tr.TimeOut1.Mins, tr.TimeOut1.AmOrPm);
                        // Calculate total working hours per each Service Code
                        if (tr.ServiceCode.Equals("032"))
                        {
                            if (calculatedHours.TotalHours < 0)
                                ts.Total032WorkedHours += TimeSpan.FromHours(24) + calculatedHours;
                            else
                                ts.Total032WorkedHours += calculatedHours;
                        }
                        else if (tr.ServiceCode.Equals("011"))
                        {
                            if (calculatedHours.TotalHours < 0)
                                ts.Total011WorkedHours += TimeSpan.FromHours(24) + calculatedHours;
                            else
                                ts.Total011WorkedHours += calculatedHours;
                        }
                       ws.Cells["AF" + RowNum.ToString()].Value = Math.Abs(tr.TotalWorkedHours.TotalHours);



                        RowNum++;


                        if (RowNum > 22)
                        {
                            RowNum = 8;
                            sheetindex++;

                            // save before go to the next sheet the total working hours per each Service Code
                            if (ts.Total032WorkedHours.TotalHours != 0)
                            {
                                ws.Cells["F24:G24"].Value = "032";
                                ws.Cells["H24:J24"].Value = Math.Abs(ts.Total032WorkedHours.TotalHours);
                            }
                            if (ts.Total011WorkedHours.TotalHours != 0)
                            {
                                 ws.Cells["F26:G26"].Value = "011";
                                ws.Cells["H26:J26"].Value = Math.Abs(ts.Total011WorkedHours.TotalHours);
                            }
                            ts.Total011WorkedHours = new TimeSpan();
                            ts.Total032WorkedHours = new TimeSpan();

                            ws = pck.Workbook.Worksheets[sheetindex];
                        }

                    }

                    if (RowNum <= 22)
                    {
                        // save in the last sheet the total working hours per each Service Code
                        if (ts.Total032WorkedHours.TotalHours != 0)
                        {
                            ws.Cells["F24:G24"].Value = "032";
                            ws.Cells["H24:J24"].Value = Math.Abs(ts.Total032WorkedHours.TotalHours);
                        }
                        if (ts.Total011WorkedHours.TotalHours != 0)
                        {
                            ws.Cells["F26:G26"].Value = "011";
                            ws.Cells["H26:J26"].Value = Math.Abs(ts.Total011WorkedHours.TotalHours);
                        }
                        ts.Total011WorkedHours = new TimeSpan();
                        ts.Total032WorkedHours = new TimeSpan();
                    }

                    string dirPath = Path.Combine(timesheetpath + @"/" + ts.EmployeeName+ "_" + ts.FromDay.Replace("/", "-") + "_" + ts.ToDay.Replace("/", "-"));
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);
                    fileName = dirPath + @"/" + ts.EmployeeName + "_" + ts.FromDay.Replace("/", "-") + "_" + ts.ToDay.Replace("/", "-") + ".xlsx";
                    // oXL.DisplayAlerts = false;
                    byte[] data = pck.GetAsByteArray();

                    File.WriteAllBytes(fileName, data);

                    //oWB.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    //    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    //    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    //oWB.Close();
                }
            }


            // oXL.ActiveWorkbook.Sheets[1].Activate();

            //  oXL.UserControl = false;

            return "";  
        }

        //static void Main(string[] args)
        //{
        //    ExcelTimeSheet p = new ExcelTimeSheet();
        //    TimeSheetExcel ts = new TimeSheetExcel();
        //    TimeExcel t = new TimeExcel();
        //    TimeRecordExcel tr = new TimeRecordExcel();
        //    TimeRecordExcel tr2 = new TimeRecordExcel();
        //    TimeRecordExcel tr3 = new TimeRecordExcel();

        //    ts.EmployeeName = "Derricka Holliday";
        //    ts.EmployeeID = "A13473";
        //    ts.ParticipantName = "Donnell Redden";
        //    ts.ParticipantID = "041668";
        //    ts.Year = "2017";
        //    ts.Phone = "407-221-2138";
        //    ts.Email = "SDTRCONSULTING@GMAIL.COM";
        //    ts.FromDay = "12/19";
        //    ts.ToDay = "12/25";

        //    tr.MonthNumber = 1;
        //    tr.Day = 3;
        //    tr.ServiceCode = "032";
        //    tr.EnterPlan = "R";
        //    tr.Backup = 'Y';
        //    t.Hours = 3;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr.TimeIn1.Hours = t.Hours;
        //    tr.TimeIn1.Mins = t.Mins;
        //    tr.TimeIn1.AmOrPm = t.AmOrPm;
        //    t.Hours = 7;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr.TimeOut1.Hours = t.Hours;
        //    tr.TimeOut1.Mins = t.Mins;
        //    tr.TimeOut1.AmOrPm = t.AmOrPm;

        //    t.Hours = 7;
        //    t.Mins = 0;
        //    t.AmOrPm = "am";
        //    tr.TimeIn2.Hours = t.Hours;
        //    tr.TimeIn2.Mins = t.Mins;
        //    tr.TimeIn2.AmOrPm = t.AmOrPm;
        //    t.Hours = 2;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr.TimeOut2.Hours = t.Hours;
        //    tr.TimeOut2.Mins = t.Mins;
        //    tr.TimeOut2.AmOrPm = t.AmOrPm;

        //   // tr.TotalWorkedHours = 11;


        //    tr2.MonthNumber = 1;
        //    tr2.Day = 3;
        //    tr2.ServiceCode = "011";
        //    tr2.EnterPlan = "S";
        //    tr2.Backup = 'Y';
        //    t.Hours = 7;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr2.TimeIn1.Hours = t.Hours;
        //    tr2.TimeIn1.Mins = t.Mins;
        //    tr2.TimeIn1.AmOrPm = t.AmOrPm;
        //    t.Hours = 8;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr2.TimeOut1.Hours = t.Hours;
        //    tr2.TimeOut1.Mins = t.Mins;
        //    tr2.TimeOut1.AmOrPm = t.AmOrPm;

        //    tr3.MonthNumber = 1;
        //    tr3.Day = 3;
        //    tr3.ServiceCode = "032";
        //    tr3.EnterPlan = "R";
        //    tr3.Backup = 'Y';
        //    t.Hours = 3;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr3.TimeIn1.Hours = t.Hours;
        //    tr3.TimeIn1.Mins = t.Mins;
        //    tr3.TimeIn1.AmOrPm = t.AmOrPm;
        //    t.Hours = 7;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr3.TimeOut1.Hours = t.Hours;
        //    tr3.TimeOut1.Mins = t.Mins;
        //    tr3.TimeOut1.AmOrPm = t.AmOrPm;

        //    t.Hours = 9;
        //    t.Mins = 0;
        //    t.AmOrPm = "am";
        //    tr3.TimeIn2.Hours = t.Hours;
        //    tr3.TimeIn2.Mins = t.Mins;
        //    tr3.TimeIn2.AmOrPm = t.AmOrPm;
        //    t.Hours = 3;
        //    t.Mins = 0;
        //    t.AmOrPm = "pm";
        //    tr3.TimeOut2.Hours = t.Hours;
        //    tr3.TimeOut2.Mins = t.Mins;
        //    tr3.TimeOut2.AmOrPm = t.AmOrPm;


        //    //tr2.TotalWorkedHours = 1;
        //    ts.LiveInEmployee = true;

        //    ts.TimeRecordsLst.Add(tr);
        //    ts.TimeRecordsLst.Add(tr2);
        //    ts.TimeRecordsLst.Add(tr3);

        //    p.FillSheet(ts);
            
        //}
    }
}
