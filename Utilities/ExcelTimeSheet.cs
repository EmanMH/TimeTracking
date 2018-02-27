﻿using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.IO;
using System.Reflection;

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
        }

        public string FillSheet(TimeSheetExcel ts,string templatepath,string timesheetpath)
        {
            Application oXL;
            _Workbook oWB;
            _Worksheet oSheet;
            object misvalue = System.Reflection.Missing.Value;
            int RowNum = 8;
            string fileName = "";

            try
            {

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();

                //Skip the Exception of the Protection Excel Template File
               // oXL.FileValidation = MsoFileValidationMode.msoFileValidationSkip;

                string path = templatepath;

                //Open the Template Sheet
                oWB = oXL.Workbooks._Open(path, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value); 

                //oXL.Visible = true;
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;


                //Fill the sheet with data
                oSheet.get_Range("C2", "N2").Value = ts.EmployeeName;
               // oSheet.get_Range("U2", "Z2").Value2 = StringtoStringArray(ts.EmployeeID);

               // oSheet.get_Range("C3", "N3").Value = ts.ParticipantName;
                //oSheet.get_Range("T3", "Z3").Value2 = StringtoStringArray(ts.ParticipantID);

              //  oSheet.get_Range("M4", "R4").Value = ts.Phone;
               // oSheet.get_Range("V4", "AF4").Value = ts.Email;

                oSheet.get_Range("C5", "F5").Value = ts.Year;
                oSheet.get_Range("L5", "S5").Value = ts.FromDay;
                oSheet.get_Range("Z5", "AF5").Value = ts.ToDay;

                if (ts.LiveInEmployee)
                    oSheet.get_Range("S28").Value = "X";
                else
                    oSheet.get_Range("W28").Value = "X";



                for (int i = 1; i < ts.TimeRecordsLst.Count / 15; i++)
                {
                    oSheet.Copy(oXL.ActiveWorkbook.Worksheets[i+1]);
                }


                foreach (TimeRecordExcel tr in ts.TimeRecordsLst ) {
                    oSheet.get_Range("A" + RowNum.ToString()).Value = tr.MonthNumber.ToString("00");
                    oSheet.get_Range("B" + RowNum.ToString()).Value = tr.Day.ToString("00");
                    oSheet.get_Range("C" + RowNum.ToString()).Value = tr.ServiceCode;
                    oSheet.get_Range("D" + RowNum.ToString() , "F" + RowNum.ToString()).Value = tr.EnterPlan.ToUpper();
                    oSheet.get_Range("G" + RowNum.ToString()).Value = tr.Backup.ToString().ToUpper();

                    oSheet.get_Range("H" + RowNum.ToString(), "I" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeIn1.Hours.ToString("00"));
                    oSheet.get_Range("J" + RowNum.ToString(), "K" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeIn1.Mins.ToString("00"));
                    oSheet.get_Range("L" + RowNum.ToString(), "M" + RowNum.ToString()).Value = tr.TimeIn1.AmOrPm.ToUpper();
                    oSheet.get_Range("N" + RowNum.ToString(), "O" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeOut1.Hours.ToString("00"));
                    oSheet.get_Range("P" + RowNum.ToString(), "Q" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeOut1.Mins.ToString("00"));
                    oSheet.get_Range("R" + RowNum.ToString(), "S" + RowNum.ToString()).Value = tr.TimeOut1.AmOrPm.ToUpper();

                   
                    tr.TotalWorkedHours = tr.TotalWorkedHours + CalculateTotalWorkingHours(tr.TimeIn1.Hours, tr.TimeIn1.Mins, tr.TimeIn1.AmOrPm
                                                                       , tr.TimeOut1.Hours, tr.TimeOut1.Mins, tr.TimeOut1.AmOrPm);

                    if (tr.TimeIn2.Hours != 0)
                    {
                        oSheet.get_Range("T" + RowNum.ToString(), "U" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeIn2.Hours.ToString("00"));
                        oSheet.get_Range("V" + RowNum.ToString(), "W" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeIn2.Mins.ToString("00"));
                        oSheet.get_Range("X" + RowNum.ToString(), "Y" + RowNum.ToString()).Value = tr.TimeIn2.AmOrPm.ToUpper();
                        oSheet.get_Range("Z" + RowNum.ToString(), "AA" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeOut2.Hours.ToString("00"));
                        oSheet.get_Range("AB" + RowNum.ToString(), "AC" + RowNum.ToString()).Value2 = StringtoStringArray(tr.TimeOut2.Mins.ToString("00"));
                        oSheet.get_Range("AD" + RowNum.ToString(), "AE" + RowNum.ToString()).Value = tr.TimeOut2.AmOrPm.ToUpper();

                        tr.TotalWorkedHours = tr.TotalWorkedHours + CalculateTotalWorkingHours(tr.TimeIn2.Hours, tr.TimeIn2.Mins, tr.TimeIn2.AmOrPm
                                                                       , tr.TimeOut2.Hours, tr.TimeOut2.Mins, tr.TimeOut2.AmOrPm);

                    }
                    oSheet.get_Range("AF" + RowNum.ToString()).Value = string.Format("{0}.{1:D2}", tr.TotalWorkedHours.Hours, tr.TotalWorkedHours.Minutes); 

                    RowNum++;
                    

                    if (RowNum > 22) {
                        RowNum = 8;
						/*oSheet.get_Range("AA26","AD26").Value = string.Format("{0}.{1:D2}", ts.Total032WorkedHours.Hours + (ts.Total032WorkedHours.Days * 24), ts.Total032WorkedHours.Minutes);
                        oSheet.get_Range("AF26").Value = string.Format("{0}.{1:D2}", ts.Total011WorkedHours.Hours + (ts.Total011WorkedHours.Days * 24), ts.Total011WorkedHours.Minutes);
                        ts.Total011WorkedHours = new TimeSpan();
                        ts.Total032WorkedHours = new TimeSpan();*/
                        
                        oSheet = oSheet.Next;
                    }

                }
				
				/*if (RowNum <= 22)
                {
                    oSheet.get_Range("AA26","AD26").Value = string.Format("{0}.{1:D2}", ts.Total032WorkedHours.Hours + (ts.Total032WorkedHours.Days * 24), ts.Total032WorkedHours.Minutes);
                    oSheet.get_Range("AF26").Value = string.Format("{0}.{1:D2}", ts.Total011WorkedHours.Hours + (ts.Total011WorkedHours.Days * 24), ts.Total011WorkedHours.Minutes);
                }*/

                oXL.ActiveWorkbook.Sheets[1].Activate();
                oXL.UserControl = false;

                string dirPath = Path.Combine(timesheetpath+ @"/" + ts.FromDay.Replace("/", "-") + "_" + ts.ToDay.Replace("/", "-"));
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                fileName = dirPath + @"/" + ts.EmployeeName + "_" + ts.FromDay.Replace("/", "-") + "_" + ts.ToDay.Replace("/", "-") + ".xls";
                oXL.DisplayAlerts = false;
                oWB.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
                return "s";
            }
            catch (Exception e)
            {
                return e.Message;
            }

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
