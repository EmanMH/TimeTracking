﻿TimeSheet = function () {
    var vm_form = function (data) {
        var self = this;
        ko.mapping.fromJS(data, null, self);
    }
    var timesheetKO;
    var url = "";
    var deletedRows = [];
    var deletedSubRows = [];
    var whichRowsDeleted=[]
    var numTimes2 = 0;

    var timesheetViewModel = function () {
        var self = this;
        var backupobj = function (Id, name) {
            this.Id = Id;
            this.name = name;
        };
        var liveInobj = function (Id, name) {
            this.Id = Id;
            this.name = name;
        };

       
       
        self.changeObject =
            {
                data: ko.observable(),
                type: ko.observable()
            }
      
        self.changes = ko.observableArray([]);
        self.current = ko.observableArray();

        self.items = ko.observableArray([]);
        self.itemsStack = ko.observableArray([]);
        self.redo=ko.observableArray([]);
        self.canRedo = ko.observableArray();
        self.serviceCodes = ko.observableArray([]);
        self.PlanSections = ko.observableArray([]);
        self.itemsOld = ko.observableArray([]);
        self.dateSelected = ko.observable();
        self.dateItems = ko.observableArray([]);
        self.backup = ko.observable();
        self.backupOld = ko.observable();
        self.liveInOld = ko.observable();
        self.liveIn = ko.observable();
        self.empname = ko.observable();
        self.Id = ko.observable();
        self.errortip = ko.observable("");
        self.HasTime2 = ko.observable();
        self.isSubmitted = ko.observable(false);
        self.isViewOnly = ko.observable(false);
        self.canUndo = ko.observable(false);
        self.isChangedA = ko.observable();
        self.ChangeCountA = ko.observable(0);
        self.isChangedT = ko.observable();
        self.ChangeCountT = ko.observable(0);
        self.setnumtimes = ko.computed(function () {
            numTimes2 = 0;
            $.each(self.items(), function (key, value1) {
                $.each(value1.times(), function (key, value) {
                    if(value.Time2()==true)
                    {
                        timesheetKO.HasTime2(true);
                        numTimes2++;
                    }
                });
            });
            if (numTimes2==0 && self.items().length!=0)
                timesheetKO.HasTime2(false);

        }, this);
        self.totalChangesCount = ko.computed(function () {
            var count = 0;
            //$.each(self.items(), function (key, value1) {
            //    $.each(value1.times(), function (key, value) {
            //        count += value.ccplansectionId();
            //        count += value.ccserviceCodeId();
            //        count += value.ccisAmIn2();
            //        count += value.ccisAmOut2();
            //        count += value.ccTimeOut2H1();
            //        count += value.ccTimeOut2M1();
            //        count += value.ccTimeIn2H1();
            //        count += value.ccTimeIn2M1();
            //        count += value.ccisAmIn();
            //        count += value.ccisAmOut();
            //        count += value.ccTimeOutH1();
            //        count += value.ccTimeOutM1();
            //        count += value.ccTimeInH1();
            //        count += value.ccTimeInM1(); 
            //    });
               
            //});
            //count += self.ChangeCountT(); //+self.ChangeCountA(); 
            return count;
    }, this);
        
        
       
        self.lessTime = function (data) {
           // numTimes2--; // decrease the number rows with time2 part

            $.each(data.times(), function (key, value) {
                value.Time2(false);
                value.TimeIn2H1(-1);
                value.TimeIn2M1(-1);
                value.isAmIn2(-1);
                value.TimeOut2H1(-1);
                value.TimeOut2M1(-1);
                value.isAmOut2(-1);
            });

            //reset dropdowns of time 2
            

            //if no more time2 part in any rows the stop display its columns
            //if (numTimes2 == 0)
            //    timesheetKO.HasTime2(false);
        }

        self.moreTime = function (data) {
           // numTimes2++; // increase the number rows with time2 part
            timesheetKO.HasTime2(true);
            $.each(data.times(), function (key, value) {
                value.Time2(true);
            });
            timesheetKO.addChange();
            
        }

        self.moreService = function (data) {
            var index = timesheetKO.items.indexOf(data);
            //copy the row

            var newRow2 = ko.mapping.fromJS(ko.mapping.toJS(data.times()[0]));
            newRow2.serviceCodeId("");
            newRow2.plansectionId("");
            newRow2
            newRow2.TimeInH1(-1);
            newRow2.TimeInM1(-1);
            newRow2
            newRow2.isAmIn(-1);
            newRow2.TimeOutH1(-1);
            newRow2.TimeOutM1(-1);
            newRow2
            newRow2
            newRow2.isAmOut(-1);
            newRow2
            newRow2.TimeIn2H1(-1);
            newRow2.TimeIn2M1(-1);
            newRow2
            newRow2.isAmIn2(-1);
            newRow2.TimeOut2H1(-1);
            newRow2.TimeOut2M1(-1);
            newRow2
            newRow2.isAmOut2(-1);
            newRow2.ccplansectionId(0);
            newRow2.ccserviceCodeId(0);
            newRow2.ccisAmIn2(0);
            newRow2.ccisAmOut2(0);
            newRow2.ccTimeOut2H1(0);
            newRow2.ccTimeOut2M1(0);
            newRow2.ccTimeIn2H1(0);
            newRow2.ccTimeIn2M1(0);
            newRow2.ccisAmIn(0);
            newRow2.ccisAmOut(0);
            newRow2.ccTimeOutH1(0);
            newRow2.ccTimeOutM1(0);
            newRow2.ccTimeInH1(0);
            newRow2.ccTimeInM1(0);
            newRow2.isAdded(true);
            // name the row not clicked by the "add time" link
            //add the copy of the row as new row in the table
            data.times.push(newRow2);
            var newRow = ko.mapping.fromJS(ko.mapping.toJS(data));

         //   timesheetKO.itemsOld()[index](newRow);


            /* reset the values of the dropdowns of the old row as the new one appear upper of it 
               in the table, so we reset it as it appear to the user as the new one */
            //data.dayName("");
            //var count = 0;
            //$.each(data.times(), function (key, value) {
            //    if (count > 0) {
            //        data.times.remove(value);
            //    }
            //    else {
            //        value.serviceCodeId("");
            //        value.plansectionId("");
            //        value
            //        value.TimeInH1(-1);
            //        value.TimeInM1(-1);
            //        value
            //        value.isAmIn(-1);
            //        value.TimeOutH1(-1);
            //        value.TimeOutM1(-1);
            //        value
            //        value
            //        value.isAmOut(-1);
            //        value
            //        value.TimeIn2H1(-1);
            //        value.TimeIn2M1(-1);
            //        value
            //        value.isAmIn2(-1);
            //        value.TimeOut2H1(-1);
            //        value.TimeOut2M1(-1);
            //        value
            //        value.isAmOut2(-1);
            //        value.ccplansectionId(0);
            //        value.ccserviceCodeId(0);
            //        value.ccisAmIn2(0);
            //        value.ccisAmOut2(0);
            //        value.ccTimeOut2H1(0);
            //        value.ccTimeOut2M1(0);
            //        value.ccTimeIn2H1(0);
            //        value.ccTimeIn2M1(0);
            //        value.ccisAmIn(0);
            //        value.ccisAmOut(0);
            //        value.ccTimeOutH1(0);
            //        value.ccTimeOutM1(0);
            //        value.ccTimeInH1(0);
            //        value.ccTimeInM1(0);
            //        value.isAdded(true);
            //        // name the row not clicked by the "add time" link
            //        value.Time2(false);
            //    }
                
            //});
            
            timesheetKO.ChangeCountT(timesheetKO.ChangeCountT()+1);
            timesheetKO.isChangedT(true);
            timesheetKO.addChange();
        }

        self.deleteRow = function (data) {
            

            if (data.times().length == 1) {
                var index = timesheetKO.items.indexOf(data);
                //deletedRows.push({ 'index': index, 'data': data });
                //whichRowsDeleted.push(-1);
                timesheetKO.canUndo(true); //enable undo button

                //if the deleted row has a time2 part then decrease the number rows with time2 part
                //if (data.times()[0].Time2() == true)
                //    numTimes2--;

                //if that was the last row with time2 part then stop display its column
                //if (numTimes2 == 0) {
                //    timesheetKO.HasTime2(false);
                //}

                if (data.times()[0].isAdded() == true) {
                    timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() - 1);
                }
                else
                    timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() + 1);

                timesheetKO.items.remove(data);
                timesheetKO.itemsOld.remove(data);
            }
            else {
                var index = timesheetKO.items.indexOf(data);
                data.times()[data.times().length - 1];
              //  deletedSubRows.push(data.times()[data.times().length - 1]);
               // whichRowsDeleted.push(index);
                timesheetKO.canUndo(true);
                data.times.remove(data.times()[data.times().length - 1]);
                var newData = ko.mapping.fromJS(ko.mapping.toJS(data));
                timesheetKO.itemsOld()[index](newData);

                if (data.times()[0].isAdded() == true) {
                    timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() - 1);
                }
                else
                    timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() + 1);
            }

            timesheetKO.addChange();

            //if (timesheetKO.ChangeCountT() == 0)
            //    timesheetKO.isChangedT(false);
            
        }
        
        self.showTime = function (data) {
            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");
            $("#backIcon").hide();
            $("#liveIcon").hide();

            $("#successdiv1").hide();
            $("#successdiv2").hide();

            var dateSelected = timesheetKO.dateSelected();
            
            timesheetKO.items([]);
            timesheetKO.itemsStack([]);
            timesheetKO.redo([]);

            timesheetKO.current(null);

            timesheetKO.itemsOld([]);
            timesheetKO.serviceCodes([]);
            timesheetKO.PlanSections([]);
            timesheetKO.backup(undefined);
            timesheetKO.liveIn(undefined);
            timesheetKO.ChangeCountA(0);
            timesheetKO.isChangedA(false);
            timesheetKO.ChangeCountT(0);
            timesheetKO.isChangedT(false);
            while (deletedRows.length != 0) {
                deletedRows.pop();
            }
            while (deletedSubRows.length != 0) {
                deletedSubRows.pop();
            }
            while (whichRowsDeleted.length != 0) {
                whichRowsDeleted.pop();
            }
            timesheetKO.canUndo(false);
            timesheetKO.canRedo(false);

            timesheetKO.isSubmitted(false);

            dateSelected = JSON.stringify({ 'dateSelected': dateSelected });
            $.ajax({
                url: window.configLocation+"/Employee/getTimeSheet",
                type: 'POST',
                data: dateSelected,
                contentType: 'application/json',
                success: function (result) {
                   // timesheetKO.items = ko.observableArray([]);
                    $.each(result.model.serviceCodes, function (key, value) {
                        timesheetKO.serviceCodes.push(new vm_form(value));
                    });
                    
                    $.each(result.model.PlanSections, function (key, value) {
                        timesheetKO.PlanSections.push(new vm_form(value));
                    });

                    //$.each(result.modelstack, function (key, value) {
                    //    timesheetKO.itemsStack.push(new vm_form(value));
                    //});


                    $.each(result.model.items, function (key, value) {
                        timesheetKO.items.push(new vm_form(value));
                        timesheetKO.itemsOld.push(new vm_form(value));

                        //count the number of rows with time2 part
                        //if (value.Time2 == true)
                        //    numTimes2++;
                    });
                    if (result.model.isBackup == null)
                        timesheetKO.backup(undefined)
                    else
                        timesheetKO.backup(result.model.isBackup ? 'Y' : 'N');
                    if (result.model.isLiveIn == null)
                        timesheetKO.liveIn(undefined)
                        else
                        timesheetKO.liveIn(result.model.isLiveIn ? 'Y' : 'N');
                    timesheetKO.liveInOld(timesheetKO.liveIn());
                    timesheetKO.backupOld(timesheetKO.backup());
                    timesheetKO.empname(result.model.empName);
                    timesheetKO.Id(result.model.Id);
                    timesheetKO.HasTime2(result.model.HasTime2);
                    timesheetKO.isViewOnly(result.model.isViewOnly);
                    timesheetKO.addChange();
                    timesheetKO.canUndo(false);
                    timesheetKO.addCurrent();
                   // $("#sheet").show();
                  //  timesheetKO.dates(result.dates);

                },
                error: function () {
                }
            });
        }
        self.validate = function (data) {
           
            $("#successdiv1").hide();
            $("#successdiv2").hide();

            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");

            $("#backIcon").hide();
            $("#liveIcon").hide();

            var str = "";
            if (data.backup() != undefined) {
                if (data.backup().trim() != "Y" && data.backup().trim() != 'N' && data.backup().trim() != "y" && data.backup().trim() != 'n') {
                    $("#backupid").css("border-color", "red");
                    $("#backIcon").show();
                    str = "123";
                }
                //str += "<p>Backup must have value Y or N</p>";
            }
            else {
                $("#backupid").css("border-color", "red");
                $("#backIcon").show();
                str = "123";


            }

            if (data.liveIn() != undefined) {
                if (data.liveIn().trim() != "Y" && data.liveIn().trim() != 'N' && data.liveIn().trim() != "y" && data.liveIn().trim() != 'n') {
                    $("#liveinid").css("border-color", "red");
                    $("#liveIcon").show();
                    str = "123";

                }
            }
            else {
                $("#liveinid").css("border-color", "red");
                $("#liveIcon").show();
                str = "123";

            }

           
            //$.each(data.items(), function (key, value) {
            //    if (value.serviceCodeId() == undefined || value.plansectionId() == undefined
            //        || value.TimeInH1() == -1  || value.TimeInM1() == -1 
            //        || value.TimeOutH1() == -1  || value.TimeOutM1() == -1 || value.isAmIn() == -1
            //        || value.isAmOut() == -1 || (value.Time2() == true && (value.TimeIn2H1() == -1 || value.TimeIn2M1() == -1
            //        || value.TimeOut2H1 == -1  || value.TimeOut2M1 == -1 ))) {
            //        str += "<p>All time sheet values must be submitted</p>";
            //        $("#errorDiv").html(str);
            //        $("#errorDiv").show();
            //        return false;
            //    }

            //    //if ((value.TimeIn() > value.TimeOut() && value.isAmIn() == true && (value.isAmOut==true || )))
            //    //    str += "<p>All time in values must be less than or equal time out</p>";

            //});
            if (str != "") {
                return false;
            }
            

            return true;
        }

        self.addChange = function () {
            
            var model = {
                items: ko.observableArray([]),
                itemsOld: ko.observableArray([]),
                backup: ko.observable(),
                liveIn: ko.observable(),
                HasTime2: ko.observable()
            };

            $.each(timesheetKO.items(), function (key, value) {
                var data2 = ko.mapping.fromJS(ko.mapping.toJS(value));
                model.items.push(data2);
               // model.itemsOld.push(data2);

               
            });
            if (timesheetKO.backup() == undefined)
                model.backup(undefined)
            else
                model.backup(timesheetKO.backup());
            if (timesheetKO.liveIn() == undefined)
                model.liveIn(undefined)
            else
                model.liveIn(timesheetKO.liveIn());
            //timesheetKO.empname(result.model.empName);
            //  timesheetKO.Id(result.model.Id);
            model.HasTime2(timesheetKO.HasTime2());
           // var model2 = ko.mapping.fromJS(model);

            timesheetKO.itemsStack.push(timesheetKO.current());
            timesheetKO.current(model);
            timesheetKO.canUndo(true);
            timesheetKO.redo([]);
            timesheetKO.canRedo(false);



        }

        self.addCurrent= function () {

            var model = {
                items: ko.observableArray([]),
                itemsOld: ko.observableArray([]),
                backup: ko.observable(),
                liveIn: ko.observable(),
                HasTime2: ko.observable()
            };

            $.each(timesheetKO.items(), function (key, value) {
                var data2 = ko.mapping.fromJS(ko.mapping.toJS(value));
                model.items.push(data2);
                // model.itemsOld.push(data2);

              
            });
            if (timesheetKO.backup() == undefined)
                model.backup(undefined)
            else
                model.backup(timesheetKO.backup());
            if (timesheetKO.liveIn() == undefined)
                model.liveIn(undefined)
            else
                model.liveIn(timesheetKO.liveIn());
            //timesheetKO.empname(result.model.empName);
            //  timesheetKO.Id(result.model.Id);
            model.HasTime2(timesheetKO.HasTime2());
            // var model2 = ko.mapping.fromJS(model);

            timesheetKO.current(model);
        }


        self.validate2 = function (data) {
            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");

            $("#backIcon").hide();
            $("#liveIcon").hide();
            timesheetKO.errortip("");
            var str = "";
            var count = 0;
            if (data.backup() != undefined) {
                if (data.backup().trim() != "Y" && data.backup().trim() != 'N' && data.backup().trim() != "y" && data.backup().trim() != 'n') {
                    $("#backupid").css("border-color", "red");
                    $("#backIcon").show();
                    str = "123";
                    timesheetKO.errortip("Backup must have value Y or N");
                    count++;
                }
                //str += "<p>Backup must have value Y or N</p>";
            }
            else {
                $("#backupid").css("border-color", "red");
                $("#backIcon").show();
                str = "123";
                timesheetKO.errortip("Backup must have value Y or N");

                count++;


            }

            if (data.liveIn() != undefined) {
                if (data.liveIn().trim() != "Y" && data.liveIn().trim() != 'N' && data.liveIn().trim() != "y" && data.liveIn().trim() != 'n') {
                    $("#liveinid").css("border-color", "red");
                    $("#liveIcon").show();
                    str = "123";
                    count++;

                    if (timesheetKO.errortip() != "")
                        timesheetKO.errortip("Backup and Live-In must have value Y or N");
                        else
                    timesheetKO.errortip("Live-In must have value Y or N");

                }
            }
            else {
                $("#liveinid").css("border-color", "red");
                $("#liveIcon").show();
                str = "123";
                count++;
                if (timesheetKO.errortip() != "")
                    timesheetKO.errortip("Backup and Live-In must have value Y or N");
                else
                    timesheetKO.errortip("Live-In must have value Y or N");

            }

            if (str != "") {
                timesheetKO.ChangeCountA(count);
                $('[data-toggle="tooltip"]').tooltip();
            }

            else {
                timesheetKO.errortip("");
                timesheetKO.ChangeCountA(0);
            }
            timesheetKO.addChange();
        }

        self.undo = function () {

            var change = timesheetKO.itemsStack.pop();
            // change = timesheetKO.itemsStack.pop();

            timesheetKO.items([]);
            timesheetKO.itemsOld([]);
            timesheetKO.backup(undefined);
            timesheetKO.liveIn(undefined);
            timesheetKO.ChangeCountA(0);
            timesheetKO.isChangedA(false);
            timesheetKO.ChangeCountT(0);
            timesheetKO.isChangedT(false);
            while (deletedRows.length != 0) {
                deletedRows.pop();
            }
            while (deletedSubRows.length != 0) {
                deletedSubRows.pop();
            }
            while (whichRowsDeleted.length != 0) {
                whichRowsDeleted.pop();
            }

            $.each(change.items(), function (key, value) {
                timesheetKO.items.push(new vm_form(value));
                timesheetKO.itemsOld.push(new vm_form(value));
            });
            if (change.backup() == undefined)
                timesheetKO.backup(undefined)
            else
                timesheetKO.backup(change.backup() ? 'Y' : 'N');
            if (change.liveIn() == undefined)
                timesheetKO.liveIn(undefined)
            else
                timesheetKO.liveIn(change.liveIn() ? 'Y' : 'N');
            timesheetKO.liveInOld(timesheetKO.liveIn());
            timesheetKO.backupOld(timesheetKO.backup());
            timesheetKO.HasTime2(change.HasTime2());

            if (timesheetKO.itemsStack().length == 1)
                timesheetKO.canUndo(false);
            timesheetKO.redo.push(timesheetKO.current());
            timesheetKO.canRedo(true);

            timesheetKO.current(change);

        }

        self.rdo = function () {

            var change = timesheetKO.redo.pop();
            // change = timesheetKO.itemsStack.pop();

            timesheetKO.items([]);
            timesheetKO.itemsOld([]);
            timesheetKO.backup(undefined);
            timesheetKO.liveIn(undefined);
            timesheetKO.ChangeCountA(0);
            timesheetKO.isChangedA(false);
            timesheetKO.ChangeCountT(0);
            timesheetKO.isChangedT(false);
            while (deletedRows.length != 0) {
                deletedRows.pop();
            }
            while (deletedSubRows.length != 0) {
                deletedSubRows.pop();
            }
            while (whichRowsDeleted.length != 0) {
                whichRowsDeleted.pop();
            }

            $.each(change.items(), function (key, value) {
                timesheetKO.items.push(new vm_form(value));
                timesheetKO.itemsOld.push(new vm_form(value));
            });
            if (change.backup() == undefined)
                timesheetKO.backup(undefined)
            else
                timesheetKO.backup(change.backup() ? 'Y' : 'N');
            if (change.liveIn() == undefined)
                timesheetKO.liveIn(undefined)
            else
                timesheetKO.liveIn(change.liveIn() ? 'Y' : 'N');
            timesheetKO.liveInOld(timesheetKO.liveIn());
            timesheetKO.backupOld(timesheetKO.backup());
            timesheetKO.HasTime2(change.HasTime2());

            if (timesheetKO.redo().length == 0)
                timesheetKO.canRedo(false);
            timesheetKO.itemsStack.push(timesheetKO.current());
            timesheetKO.canUndo(true);

            timesheetKO.current(change);

        }
        self.saveDraft = function (data) {

            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");
            $("#backIcon").hide();
            $("#liveIcon").hide();
            $("#successdiv1").hide();
            $("#successdiv1").hide();

            $("#loading1").html("Saving...");
            $("#loading2").html("Saving...");

            $("#loading1").show();
            $("#loading2").show();

                var items = ko.toJSON(data.items());
                //var backup = ko.toJSON(data.backup());
                //var livein = ko.toJSON(data.liveIn());
                var data = JSON.stringify({ 'items': items, 'backup': data.backup() != undefined ? data.backup().trim() : undefined, 'livein': data.liveIn() != undefined ? data.liveIn().trim() : undefined, 'draft': true, 'id': data.Id() });
                $.ajax({
                    url: window.configLocation + "/Employee/saveTimeSheet",
                    type: 'POST',
                    data: data,
                    contentType: 'application/json',
                    success: function (result) {
                        timesheetKO.Id(result);
                        $("#successdiv1").html("Saved Successfully");
                        $("#successdiv2").html("Saved Successfully");


                        $("#successdiv1").show();
                        $("#successdiv2").show();

                       // $("#successModal").modal('show');
                        $("#containerBody").html("Saved Successfully");

                        $("#loading1").hide();
                        $("#loading2").hide();

                        timesheetKO.ChangeCountA(0);
                        timesheetKO.isChangedA(false);
                        timesheetKO.ChangeCountT(0);
                        timesheetKO.isChangedT(false);
                        timesheetKO.itemsOld([]);
                        $.each(timesheetKO.items(), function (key, value) {
                            var data2 = ko.mapping.fromJS(ko.mapping.toJS(value));
                            timesheetKO.itemsOld.push(data2);
                            $.each(value.times(), function (key, value2) {
                                value2.ccplansectionId(0);
                                value2.ccserviceCodeId(0);
                                value2.ccisAmIn2(0);
                                value2.ccisAmOut2(0);
                                value2.ccTimeOut2H1(0);
                                value2.ccTimeOut2M1(0);
                                value2.ccTimeIn2H1(0);
                                value2.ccTimeIn2M1(0);
                                value2.ccisAmIn(0);
                                value2.ccisAmOut(0);
                                value2.ccTimeOutH1(0);
                                value2.ccTimeOutM1(0);
                                value2.ccTimeInH1(0);
                                value2.ccTimeInM1(0); 
                            });
                        });
                        timesheetKO.itemsStack([]);
                        timesheetKO.redo([]);
                        timesheetKO.canRedo(false);


                    },
                    error: function (result) {
                        $("#successdiv1").hide();
                        $("#successdiv2").hide();

                      //  $("#successModal").modal('hide');
                        $("#loading1").hide();
                        $("#loading2").hide();


                        //error$("#successdive").html("");

                    }
                });
        }

        self.save = function (data) {
            $("#successdiv1").hide();
            $("#successdiv2").hide();

           // $("#successModal").modal('hide');

            while (deletedRows.length != 0) {
                deletedRows.pop();
            }
            while (deletedSubRows.length != 0) {
                deletedSubRows.pop();
            }
            while (whichRowsDeleted.length != 0) {
                whichRowsDeleted.pop();
            }
            timesheetKO.canUndo(false);

            if (timesheetKO.validate(data)) {
                $("#loading1").html("Submitting...");
                $("#loading2").html("Submitting...");


                $("#loading1").show();
                $("#loading2").show();


                var items = ko.toJSON(data.items());
                //var backup = ko.toJSON(data.backup());
                //var livein = ko.toJSON(data.liveIn());
                var data = JSON.stringify({ 'items': items, 'backup': data.backup().trim(), 'livein': data.liveIn().trim(), 'draft': false, 'id': data.Id() });
                $.ajax({
                    url: window.configLocation+ "/Employee/saveTimeSheet",
                    type: 'POST',
                    data: data,
                    contentType: 'application/json',
                    success: function (result) {
                        timesheetKO.Id(result);

                        $("#successdiv1").html("Submitted Successfully");
                        $("#successdiv2").html("Submitted Successfully");


                        $("#successdiv1").show();
                        $("#successdiv2").show();

                       // $("#successModal").modal('show');
                        $("#containerBody").html("Submitted Successfully");
                        $("#loading1").hide();
                        $("#loading2").hide();

                        timesheetKO.isSubmitted(true);
                        timesheetKO.ChangeCountA(0);
                        timesheetKO.isChangedA(false);
                        timesheetKO.ChangeCountT(0);
                        timesheetKO.isChangedT(false);
                        $.each(timesheetKO.items(), function (key, value) {
                            $.each(value.times(), function (key, value2) {
                                value2.ccplansectionId(0);
                                value2.ccserviceCodeId(0);
                                value2.ccisAmIn2(0);
                                value2.ccisAmOut2(0);
                                value2.ccTimeOut2H1(0);
                                value2.ccTimeOut2M1(0);
                                value2.ccTimeIn2H1(0);
                                value2.ccTimeIn2M1(0);
                                value2.ccisAmIn(0);
                                value2.ccisAmOut(0);
                                value2.ccTimeOutH1(0);
                                value2.ccTimeOutM1(0);
                                value2.ccTimeInH1(0);
                                value2.ccTimeInM1(0);
                            });
                            
                        });
                        timesheetKO.itemsStack([]);
                        timesheetKO.redo([]);
                        timesheetKO.canRedo(false);


                    },
                    error: function (result) {
                        $("#successdiv1").hide();
                        $("#successdiv2").hide();

                       // $("#successModal").modal('hide');
                        $("#loading1").show();
                        $("#loading2").show();


                        //error$("#successdive").html("");

                    }
                });
            }
        }

        self.changedbl = function (data) {
            timesheetKO.changeObject.data(data);
            timesheetKO.changeObject.type("backuplivein");
            var newCh = ko.mapping.fromJS(ko.mapping.toJS(timesheetKO.changeObject));
            timesheetKO.changes.push(newCh);
            timesheetKO.canUndo(true);
            //undefined and "" comparision
            if (data.backup() == "")
                data.backup(undefined);
            if (data.liveIn() == "")
                data.liveIn(undefined);
            else if (data.backup() == timesheetKO.backupOld() && data.liveIn() == timesheetKO.liveInOld()) {
                timesheetKO.isChangedA(false);
                timesheetKO.ChangeCountA(0);
            }
            else if (data.backup() != timesheetKO.backupOld() && data.liveIn() == timesheetKO.liveInOld()) {
                timesheetKO.isChangedA(true);
                timesheetKO.ChangeCountA(1);
            }
            else if (data.backup() != timesheetKO.backupOld() && data.liveIn() != timesheetKO.liveInOld()) {
                timesheetKO.isChangedA(true);
                timesheetKO.ChangeCountA(2);
            }
            else if (data.backup() == timesheetKO.backupOld() && data.liveIn() != timesheetKO.liveInOld()) {
                timesheetKO.isChangedA(true);
                timesheetKO.ChangeCountA(1);
            }
            else {
                timesheetKO.isChangedA(false);
                timesheetKO.ChangeCountA(0);
            }

        }

        self.changedItemSVC = function (data) {
            if (timesheetKO.isSubmitted() != true) {
                var index = timesheetKO.items.indexOf(data);
                if (data.serviceCodeId() != timesheetKO.itemsOld()[index].serviceCodeId()) {
                    data.ccserviceCodeId(1);
                }
                else
                    data.ccserviceCodeId(0);
            }
        }

        self.changedItemPlan = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.plansectionId() != timesheetKO.itemsOld()[index].plansectionId()) {
                    data.ccplansectionId(1);
                }
                else
                    data.ccplansectionId(0);
            }
        }

        self.changedItemTimeInH1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeInH1() != timesheetKO.itemsOld()[index].TimeInH1()) {
                    data.ccTimeInH1(1);
                }
                else
                    data.ccTimeInH1(0);
            }
        }

        self.changedItemTimeInM1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeInM1() != timesheetKO.itemsOld()[index].TimeInM1()) {
                    data.ccTimeInM1(1);
                }
                else
                    data.ccTimeInM1(0);
            }
        }

        self.changedItemisAmIn = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.isAmIn() != timesheetKO.itemsOld()[index].isAmIn()) {
                    data.ccisAmIn(1);
                }
                else
                    data.ccisAmIn(0);
            }
        }

        self.changedItemisAmIn = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.isAmIn() != timesheetKO.itemsOld()[index].isAmIn()) {
                    data.ccisAmIn(1);
                }
                else
                    data.ccisAmIn(0);
            }
        }

        self.changedItemTimeOutH1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeOutH1() != timesheetKO.itemsOld()[index].TimeOutH1()) {
                    data.ccTimeOutH1(1);
                }
                else
                    data.ccTimeOutH1(0);
            }
        }
        self.changedItemTimeOutM1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeOutM1() != timesheetKO.itemsOld()[index].TimeOutM1()) {
                    data.ccTimeOutM1(1);
                }
                else
                    data.ccTimeOutM1(0);
            }
        }

        self.changedItemisAmOut = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.isAmOut() != timesheetKO.itemsOld()[index].isAmOut()) {
                    data.ccisAmOut(1);
                }
                else
                    data.ccisAmOut(0);
            }
        }

        self.changedItemTimeIn2H1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeIn2H1() != timesheetKO.itemsOld()[index].TimeIn2H1()) {
                    data.ccTimeIn2H1(1);
                }
                else
                    data.ccTimeIn2H1(0);
            }
        }

        self.changedItemTimeIn2M1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeIn2M1() != timesheetKO.itemsOld()[index].TimeIn2M1()) {
                    data.ccTimeIn2M1(1);
                }
                else
                    data.ccTimeIn2M1(0);
            }
        }

        self.changedItemisAmIn2 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.isAmIn2() != timesheetKO.itemsOld()[index].isAmIn2()) {
                    data.ccisAmIn2(1);
                }
                else
                    data.ccisAmIn2(0);
            }
        }

        self.changedItemTimeOut2H1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeOut2H1() != timesheetKO.itemsOld()[index].TimeOut2H1()) {
                    data.ccTimeOut2H1(1);
                }
                else
                    data.ccTimeOut2H1(0);
            }
        }

        self.changedItemTimeOut2M1 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.TimeOut2M1() != timesheetKO.itemsOld()[index].TimeOut2M1()) {
                    data.ccTimeOut2M1(1);
                }
                else
                    data.ccTimeOut2M1(0);
            }
        }

        self.changedItemisAmOut2 = function (data) {
            if (timesheetKO.isSubmitted() != true) {

                var index = timesheetKO.items.indexOf(data);
                if (data.isAmOut2() != timesheetKO.itemsOld()[index].isAmOut2()) {
                    data.ccisAmOut2(1);
                }
                else
                    data.ccisAmOut2(0);
            }
        }
    }

  

    var load = function () {
        timesheetKO = new timesheetViewModel();
        ko.applyBindings(timesheetKO);
       // timesheetKO.showTime();
        $.ajax({
            url: window.configLocation+"/Employee/loadDates",
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {
                $.each(result, function (key, value) {
                    timesheetKO.dateItems.push(new vm_form(value));
                });
            },
            error: function () {
            }
        });
       
    };

    return {
        load: load
    }
}();