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
        self.backuparr = ko.observableArray([new backupobj(1, 'Yes'), new backupobj(2,'No')]);
        self.liveInarr = ko.observableArray([new liveInobj(1, 'Yes'), new liveInobj(2, 'No')]);
        self.backup = ko.observable();
        self.liveIn = ko.observable();
        self.empname = ko.observable();
        self.HasTime2 = ko.observable();

        self.moreTime = function (data) {
            timesheetKO.HasTime2(true);
            data.Time2(true);
        }

        self.moreService = function (data) {
            var index = timesheetKO.items.indexOf(data);
            timesheetKO.items.splice(index, 0, data);
        }
       
        self.showTime = function (data) {
            debugger;
            var dateSelected = timesheetKO.dateSelected();
            dateSelected = JSON.stringify({ 'dateSelected': dateSelected });
            $.ajax({
                url: "/Employee/getTimeSheet",
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
                    timesheetKO.backup(result.isBackup);
                    timesheetKO.liveIn(result.isLiveIn);
                    timesheetKO.empname(result.empName);
                    timesheetKO.HasTime2(result.HasTime2);

                },
                error: function () {
                }
            });
        }

        self.save = function (data) {
            var items = JSON.stringify({ 'items': data });
            $.ajax({
                url: "/Employee/saveTimeSheet",
                type: 'POST',
                data: items,
                contentType: 'application/json',
                success: function (result) {
                    $.each(result.timesheets.items, function (key, value) {
                        timesheetKO.items.push(new vm_form(value));
                    });
                    $.each(result.timesheets.serviceCodes, function (key, value) {
                        timesheetKO.serviceCodes.push(new vm_form(value));
                    });
                    $.each(result.timesheets.PlanSections, function (key, value) {
                        timesheetKO.PlanSections.push(new vm_form(value));
                    });
                    timesheetKO.backup(result.timesheets.isBackup);
                    timesheetKO.liveIn(result.timesheets.isLiveIn);
                    timesheetKO.empname(result.timesheets.empName);
                    timesheetKO.HasTime2(result.timesheets.HasTime2);

                },
                error: function () {
                }
            });

        }

    }

    var load = function () {
        timesheetKO = new timesheetViewModel();
        ko.applyBindings(timesheetKO);

       // timesheetKO.showTime();
        $.ajax({
            url: "/Employee/loadDates",
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