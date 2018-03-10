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

        self.dateSelected = ko.observable();
        self.dateItems = ko.observableArray([]);
        self.backup = ko.observable();
        self.liveIn = ko.observable();
        self.empname = ko.observable();
        self.Id = ko.observable();

        self.HasTime2 = ko.observable();

        self.isViewOnly = ko.observable(false);
        self.canUndo = ko.observable(false);

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

            // name the row not clicked by the "add time" link
            data.Time2(false);
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

            timesheetKO.items.remove(data);
        }
        
        self.showTime = function (data) {
            $("#errorDiv").hide();
            $("#successdiv").hide();
            var dateSelected = timesheetKO.dateSelected();
            timesheetKO.items([]);
            timesheetKO.serviceCodes([]);
            timesheetKO.PlanSections([]);
            deletedRows = [];
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
                        //count the number of rows with time2 part
                        if (value.Time2 == true)
                            numTimes2++;
                    });
                    
                  //  timesheetKO.backup(result.isBackup:'Y':'N');
                   // timesheetKO.liveIn(result.isLiveIn);
                    timesheetKO.empname(result.empName);
                    timesheetKO.Id(result.Id);
                    timesheetKO.HasTime2(result.HasTime2);
                    timesheetKO.isViewOnly(result.isViewOnly);
                  //  timesheetKO.dates(result.dates);

                },
                error: function () {
                }
            });
        }
        self.validate = function (data) {
            $("#errorDiv").hide();
            $("#successdiv").hide();

            var str = "";
            if (data.backup() != "Y" && data.backup() != 'N')
                str += "<p>Backup must have value Y or N</p>";
            if (data.liveIn() != "Y" && data.liveIn() != 'N')
                str += "<p>Live-In must have value Y or N</p>";
            $.each(data.items(), function (key, value) {
                if (value.serviceCodeId() == undefined || value.plansectionId() == undefined
                    || value.TimeInH1() == -1  || value.TimeInM1() == -1 
                    || value.TimeOutH1() == -1  || value.TimeOutM1() == -1 || value.isAmIn() == -1
                    || value.isAmOut() == -1 || (value.Time2() == true && (value.TimeIn2H1() == -1 || value.TimeIn2M1() == -1
                    || value.TimeOut2H1 == -1  || value.TimeOut2M1 == -1 ))) {
                    str += "<p>All time sheet values must be submitted</p>";
                    $("#errorDiv").html(str);
                    $("#errorDiv").show();
                    return false;
                }

                //if ((value.TimeIn() > value.TimeOut() && value.isAmIn() == true && (value.isAmOut==true || )))
                //    str += "<p>All time in values must be less than or equal time out</p>";

            });
            if (str != "") {
                $("#errorDiv").html(str);
                $("#errorDiv").show();
                return false;
            }
            else
                $("#errorDiv").hide();

            return true;
        }

        self.undo = function (data) {
            var row = deletedRows.pop();
            timesheetKO.items.splice(row.index, 0, row.data);

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
            $("#successdiv").hide();

                var items = ko.toJSON(data.items());
                //var backup = ko.toJSON(data.backup());
                //var livein = ko.toJSON(data.liveIn());
                var data = JSON.stringify({ 'items': items, 'backup': data.backup(), 'livein': data.liveIn(),'draft':true,'id':data.Id() });
                $.ajax({
                    url: window.configLocation + "/Employee/saveTimeSheet",
                    type: 'POST',
                    data: data,
                    contentType: 'application/json',
                    success: function (result) {
                        $("#successdiv").show();
                        $("#successModal").modal('show');

                    },
                    error: function (result) {
                        $("#successdiv").hide();
                        $("#successModal").modal('hide');

                        //error$("#successdive").html("");

                    }
                });
        }

        self.save = function (data) {
            $("#successdiv").hide();
            $("#successModal").modal('hide');

            deletedRows = [];
            if (timesheetKO.validate(data)) {
                var items = ko.toJSON(data.items());
                //var backup = ko.toJSON(data.backup());
                //var livein = ko.toJSON(data.liveIn());
                var data = JSON.stringify({ 'items': items, 'backup': data.backup(), 'livein': data.liveIn(), 'draft': false, 'id': data.Id() });
                $.ajax({
                    url: window.configLocation+ "/Employee/saveTimeSheet",
                    type: 'POST',
                    data: data,
                    contentType: 'application/json',
                    success: function (result) {
                        $("#successdiv").show();
                        $("#successModal").modal('show');

                        

                    },
                    error: function (result) {
                        $("#successdiv").hide();
                        $("#successModal").modal('hide');

                        //error$("#successdive").html("");

                    }
                });
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