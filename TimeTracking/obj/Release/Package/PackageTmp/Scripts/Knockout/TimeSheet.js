TimeSheet = function () {
    var vm_form = function (data) {
        var self = this;
        ko.mapping.fromJS(data, null, self);
    }
    var timesheetKO;
    var url = "";
    var deletedRows = [];
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
        self.items = ko.observableArray([]);
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

        self.HasTime2 = ko.observable();
        self.isSubmitted = ko.observable(false);
        self.isViewOnly = ko.observable(false);
        self.canUndo = ko.observable(false);
        self.isChangedA = ko.observable();
        self.ChangeCountA = ko.observable(0);
        self.isChangedT = ko.observable();
        self.ChangeCountT = ko.observable(0);
        self.totalChangesCount = ko.computed(function () {
            debugger;
            var count = 0;
            $.each(self.items(), function (key, value) {
                count += value.ccplansectionId();
                count += value.ccserviceCodeId();
                count += value.ccisAmIn2();
                count += value.ccisAmOut2();
                count += value.ccTimeOut2H1();
                count += value.ccTimeOut2M1();
                count += value.ccTimeIn2H1();
                count += value.ccTimeIn2M1();
                count += value.ccisAmIn();
                count += value.ccisAmOut();
                count += value.ccTimeOutH1();
                count += value.ccTimeOutM1();
                count += value.ccTimeInH1();
                count += value.ccTimeInM1(); 
            });
            count += self.ChangeCountA() + self.ChangeCountT();
            return count;
    }, this);
        
        
       
        self.lessTime = function (data) {
            numTimes2--; // decrease the number rows with time2 part

            data.Time2(false);

            //reset dropdowns of time 2
            data.TimeIn2H1(-1);
            data.TimeIn2M1(-1);
            data.isAmIn2(-1);
            data.TimeOut2H1(-1);
            data.TimeOut2M1(-1);
            data.isAmOut2(-1);

            //if no more time2 part in any rows the stop display its columns
            if (numTimes2 == 0)
                timesheetKO.HasTime2(false);
        }

        self.moreTime = function (data) {
            numTimes2++; // increase the number rows with time2 part
            timesheetKO.HasTime2(true);
            data.Time2(true);
        }

        self.moreService = function (data) {
            var index = timesheetKO.items.indexOf(data);
            //copy the row
            var newRow = ko.mapping.fromJS(ko.mapping.toJS(data));
            //add the copy of the row as new row in the table
            timesheetKO.items.splice(index, 0, newRow);
            timesheetKO.itemsOld.splice(index, 0, newRow);


            /* reset the values of the dropdowns of the old row as the new one appear upper of it 
               in the table, so we reset it as it appear to the user as the new one */
            data.serviceCodeId("");
            data.plansectionId("");

            data.TimeInH1(-1);
            data.TimeInM1(-1);

            data.isAmIn(-1);
            data.TimeOutH1(-1);
            data.TimeOutM1(-1);


            data.isAmOut(-1);

            data.TimeIn2H1(-1);
            data.TimeIn2M1(-1);

            data.isAmIn2(-1);
            data.TimeOut2H1(-1);
            data.TimeOut2M1(-1);

            data.isAmOut2(-1);
            data.ccplansectionId(0);
            data.ccserviceCodeId(0);
            data.ccisAmIn2(0);
            data.ccisAmOut2(0);
            data.ccTimeOut2H1(0);
            data.ccTimeOut2M1(0);
            data.ccTimeIn2H1(0);
            data.ccTimeIn2M1(0);
            data.ccisAmIn(0);
            data.ccisAmOut(0);
            data.ccTimeOutH1(0);
            data.ccTimeOutM1(0);
            data.ccTimeInH1(0);
            data.ccTimeInM1(0); 
            data.isAdded(true);
            debugger;
            // name the row not clicked by the "add time" link
            data.Time2(false);
            timesheetKO.ChangeCountT(timesheetKO.ChangeCountT()+1);
            timesheetKO.isChangedT(true);
        }

        self.deleteRow = function (data) {
            var index = timesheetKO.items.indexOf(data);
            deletedRows.push({ 'index':index,'data': data });
            timesheetKO.canUndo(true); //enable undo button

            //if the deleted row has a time2 part then decrease the number rows with time2 part
            if (data.Time2() == true) 
                numTimes2--;

            //if that was the last row with time2 part then stop display its column
            if (numTimes2 == 0) {
                timesheetKO.HasTime2(false);
            }

            if (data.isAdded() == true)
            {
                timesheetKO.ChangeCountT(timesheetKO.ChangeCountT()-1);
            }
            else
                timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() + 1);

            timesheetKO.items.remove(data);
            timesheetKO.itemsOld.remove(data);

            //if (timesheetKO.ChangeCountT() == 0)
            //    timesheetKO.isChangedT(false);
            
        }
        
        self.showTime = function (data) {
            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");
            $("#backIcon").hide();
            $("#liveIcon").hide();

            $("#successdiv").hide();
            var dateSelected = timesheetKO.dateSelected();
            timesheetKO.items([]);
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
            timesheetKO.canUndo(false);
            timesheetKO.isSubmitted(false);
            dateSelected = JSON.stringify({ 'dateSelected': dateSelected });
            $.ajax({
                url: window.configLocation+"/Employee/getTimeSheet",
                type: 'POST',
                data: dateSelected,
                contentType: 'application/json',
                success: function (result) {
                   // timesheetKO.items = ko.observableArray([]);
                    $.each(result.serviceCodes, function (key, value) {
                        timesheetKO.serviceCodes.push(new vm_form(value));
                    });
                    
                    $.each(result.PlanSections, function (key, value) {
                        timesheetKO.PlanSections.push(new vm_form(value));
                    });

                    $.each(result.items, function (key, value) {
                        timesheetKO.items.push(new vm_form(value));
                        timesheetKO.itemsOld.push(new vm_form(value));

                        //count the number of rows with time2 part
                        if (value.Time2 == true)
                            numTimes2++;
                    });
                    if (result.isBackup == null)
                        timesheetKO.backup(undefined)
                    else
                        timesheetKO.backup(result.isBackup ? 'Y' : 'N');
                    if (result.isLiveIn == null)
                        timesheetKO.liveIn(undefined)
                        else
                        timesheetKO.liveIn(result.isLiveIn ? 'Y' : 'N');
                    timesheetKO.liveInOld(timesheetKO.liveIn());
                    timesheetKO.backupOld(timesheetKO.backup());
                    timesheetKO.empname(result.empName);
                    timesheetKO.Id(result.Id);
                    timesheetKO.HasTime2(result.HasTime2);
                    timesheetKO.isViewOnly(result.isViewOnly);
                   // $("#sheet").show();
                  //  timesheetKO.dates(result.dates);

                },
                error: function () {
                }
            });
        }
        self.validate = function (data) {
           
            $("#successdiv").hide();
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

        self.undo = function (data) {
            var row = deletedRows.pop();
            timesheetKO.items.splice(row.index, 0, row.data);
            var data2 = ko.mapping.fromJS(ko.mapping.toJS(row.data));
            timesheetKO.itemsOld.splice(row.index, 0, data2);

            timesheetKO.ChangeCountT(timesheetKO.ChangeCountT() - 1);
            if (timesheetKO.ChangeCountT() == 0)
                timesheetKO.isChangedT(false);
           

            //check flog of Time 2 part to display it
            if (row.data.Time2() == true)
                numTimes2++;

            if (numTimes2 > 0) {
                timesheetKO.HasTime2(true);
            }

            //if no more row to undo then disable the undo button
            if (deletedRows.length == 0)
                timesheetKO.canUndo(false);
        }
        self.saveDraft = function (data) {

            $("#backupid").css("border-color", "");
            $("#liveinid").css("border-color", "");
            $("#backIcon").hide();
            $("#liveIcon").hide();
            $("#successdiv").hide();
            $("#loading").html("Saving...");
            $("#loading").show();
            $("#successdiv").hide();
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
                        $("#successdiv").html("Saved Successfully");

                        $("#successdiv").show();
                       // $("#successModal").modal('show');
                        $("#containerBody").html("Saved Successfully");

                        $("#loading").hide();
                        timesheetKO.ChangeCountA(0);
                        timesheetKO.isChangedA(false);
                        timesheetKO.ChangeCountT(0);
                        timesheetKO.isChangedT(false);
                        timesheetKO.itemsOld([]);
                        timesheetKO.itemsOld(timesheetKO.items());
                        $.each(timesheetKO.items(), function (key, value) {
                            value.ccplansectionId(0);
                            value.ccserviceCodeId(0);
                            value.ccisAmIn2(0);
                            value.ccisAmOut2(0);
                            value.ccTimeOut2H1(0);
                            value.ccTimeOut2M1(0);
                            value.ccTimeIn2H1(0);
                            value.ccTimeIn2M1(0);
                            value.ccisAmIn(0);
                            value.ccisAmOut(0);
                            value.ccTimeOutH1(0);
                            value.ccTimeOutM1(0);
                            value.ccTimeInH1(0);
                            value.ccTimeInM1(0); 
                        });

                    },
                    error: function (result) {
                        $("#successdiv").hide();
                      //  $("#successModal").modal('hide');
                        $("#loading").hide();

                        //error$("#successdive").html("");

                    }
                });
        }

        self.save = function (data) {
            $("#successdiv").hide();
           // $("#successModal").modal('hide');

            while (deletedRows.length != 0) {
                deletedRows.pop();
            }
            timesheetKO.canUndo(false);

            if (timesheetKO.validate(data)) {
                $("#loading").html("Submitting...");

                $("#loading").show();

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

                        $("#successdiv").html("Submitted Successfully");

                        $("#successdiv").show();
                       // $("#successModal").modal('show');
                        $("#containerBody").html("Submitted Successfully");
                        $("#loading").hide();
                        timesheetKO.isSubmitted(true);
                        timesheetKO.ChangeCountA(0);
                        timesheetKO.isChangedA(false);
                        timesheetKO.ChangeCountT(0);
                        timesheetKO.isChangedT(false);
                        $.each(timesheetKO.items(), function (key, value) {
                            value.ccplansectionId(0);
                            value.ccserviceCodeId(0);
                            value.ccisAmIn2(0);
                            value.ccisAmOut2(0);
                            value.ccTimeOut2H1(0);
                            value.ccTimeOut2M1(0);
                            value.ccTimeIn2H1(0);
                            value.ccTimeIn2M1(0);
                            value.ccisAmIn(0);
                            value.ccisAmOut(0);
                            value.ccTimeOutH1(0);
                            value.ccTimeOutM1(0);
                            value.ccTimeInH1(0);
                            value.ccTimeInM1(0);
                        });

                    },
                    error: function (result) {
                        $("#successdiv").hide();
                       // $("#successModal").modal('hide');
                        $("#loading").show();

                        //error$("#successdive").html("");

                    }
                });
            }
        }

        self.changedbl = function (data) {
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