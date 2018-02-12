TimeSheet = function () {
    var vm_form = function (data) {
        var self = this;
        ko.mapping.fromJS(data, null, self);
    }
    var timesheetKO;
    var url = "";

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
        self.HasTime2 = ko.observable();
        self.isViewOnly = ko.observable(false);

        self.moreTime = function (data) {
            timesheetKO.HasTime2(true);
            data.Time2(true);
        }

        self.moreService = function (data) {
            var index = timesheetKO.items.indexOf(data);
            timesheetKO.items.splice(index, 0, data);
        }
       
        self.showTime = function (data) {
            var dateSelected = timesheetKO.dateSelected();
            timesheetKO.items([]);
            timesheetKO.serviceCodes([]);
            timesheetKO.PlanSections([]);

            dateSelected = JSON.stringify({ 'dateSelected': dateSelected });
            $.ajax({
                url: window.configLocation+"/Employee/getTimeSheet",
                type: 'POST',
                data: dateSelected,
                contentType: 'application/json',
                success: function (result) {
                   // timesheetKO.items = ko.observableArray([]);
                    $.each(result.items, function (key, value) {
                        timesheetKO.items.push(new vm_form(value));
                    });
                    $.each(result.serviceCodes, function (key, value) {
                        timesheetKO.serviceCodes.push(new vm_form(value));
                    });
                    $.each(result.PlanSections, function (key, value) {
                        timesheetKO.PlanSections.push(new vm_form(value));
                    });
                  //  timesheetKO.backup(result.isBackup:'Y':'N');
                   // timesheetKO.liveIn(result.isLiveIn);
                    timesheetKO.empname(result.empName);
                    timesheetKO.HasTime2(result.HasTime2);
                    timesheetKO.isViewOnly(result.isViewOnly);


                },
                error: function () {
                }
            });
        }
        self.validate = function (data) {
            var str = "";
            if (data.backup() != "Y" && data.backup() != 'N')
                str += "<p>Backup must have value Y or N</p>";
            if (data.liveIn() != "Y" && data.liveIn() != 'N')
                str += "<p>Live-In must have value Y or N</p>";
            $.each(data.items(), function (key, value) {
                if (value.serviceCodeId() == undefined || value.plansectionId() == undefined
                    || value.TimeIn() == -1 || value.TimeOut() == -1 || value.isAmIn() == -1
                    || value.isAmOut() == -1 || (value.Time2() == true && (value.TimeIn2() == -1 || value.TimeOut2 == -1))) {
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
        self.save = function (data) {
            debugger;
            if (timesheetKO.validate(data)) {
                var items = ko.toJSON(data.items());
                //var backup = ko.toJSON(data.backup());
                //var livein = ko.toJSON(data.liveIn());
                var data = JSON.stringify({ 'items': items, 'backup': data.backup(), 'livein': data.liveIn() });
                $.ajax({
                    url: window.configLocation+ "/Employee/saveTimeSheet",
                    type: 'POST',
                    data: data,
                    contentType: 'application/json',
                    success: function (result) {
                        $("#successdiv").show();
                        

                    },
                    error: function (result) {
                        $("#successdiv").hide();
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