Logs = function () {
    var logsKO;

    var item = function (name, hasM, hasA, hasE, hasV, mLst, aLst, eLst, vLst) {
        self.name = name;
        self.hasM = hasM;
        self.hasA = hasA;
        self.hasE = hasE;
        self.hasV = hasV;

        self.mLst = ko.observableArray(mLst);
        self.aLst = ko.observableArray(aLst);
        self.eLst = ko.observableArray(eLst);
        self.vLst = ko.observableArray(vLst);
    };

    var CategorizeItem = function (cName, cLst) {
        self.cName = cName;
        self.cLst = ko.observableArray(cLst);
    };

    var logsViewModel = function () {
        var self = this;
        
        self.DAlst = ko.observableArray([]);
        self.ctgDAlst = ko.observableArray([]);
        self.swpdDAlst = ko.observableArray([]);
        self.isDASwpd = ko.observable(false);

        self.FPlst = ko.observableArray([]);
        self.ctgFPlst = ko.observableArray([]);
        self.swpdFPlst = ko.observableArray([]);
        self.isFPSwpd = ko.observable(false);

        self.Alst = ko.observableArray([]);
        self.ctgAlst = ko.observableArray([]);
        self.swpdAlst = ko.observableArray([]);
        self.isASwpd = ko.observable(false);

        self.LcHklst = ko.observableArray([]);
        self.ctgLcHklst = ko.observableArray([]);
        self.swpdLcHklst = ko.observableArray([]);
        self.isLcHkSwpd = ko.observable(false);

        self.isChecked = ko.observableArray([]);
        
        self.saveLogs = function (data) {
            $("#successdiv").hide();
            $("#successModal").modal('hide');

            $("#loading").html("Submitting...");

            $("#loading").show();

            var ids = ko.toJSON(data.isChecked());
            var dataS = JSON.stringify({ 'checkedItems': ids });
            $.ajax({
                url: window.configLocation + "/Employee/saveLogs",
                type: 'POST',
                data: dataS,
                contentType: 'application/json',
                success: function (result) {
                    $("#successdiv").html("Submitted Successfully");

                    $("#successdiv").show();
                    $("#successModal").modal('show');
                    $("#containerBody").html("Submitted Successfully");
                    $("#loading").hide();
                },
                error: function (result) {
                    $("#successdiv").hide();
                    $("#successModal").modal('hide');
                    $("#loading").show();
                }
            });
            
        };
    };

    var load = function () {
        logsKO = new logsViewModel();
        ko.applyBindings(logsKO);

        $.ajax({
            url: window.configLocation + "/Employee/loadLogItems",
            type: 'POST',
            contentType: 'application/json',
            success: function (result) {

                $.each(result.selectedLogs, function (key, value) {
                    logsKO.isChecked.push(value);
                }),

                $.each(result.DLActivities.logsLst, function (key, value) {
                    logsKO.DAlst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.DLActivities.logSwapLst, function (key, value) {
                    logsKO.isDASwpd(true);
                    logsKO.swpdDAlst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.DLActivities.logCtgLst, function (key, value) {
                    logsKO.ctgDAlst.push(new CategorizeItem(value.cateogryName, value.logLst));
                }),

                $.each(result.FPActivities.logsLst, function (key, value) {
                    logsKO.FPlst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.FPActivities.logSwapLst, function (key, value) {
                    logsKO.isFPSwpd(true);
                    logsKO.swpdFPlst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.FPActivities.logCtgLst, function (key, value) {
                   logsKO.ctgFPlst.push(new CategorizeItem(value.cateogryName, value.logLst));
                }),

                $.each(result.Activities.logsLst, function (key, value) {
                    logsKO.Alst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.Activities.logSwapLst, function (key, value) {
                    logsKO.isASwpd(true);
                    logsKO.swpdAlst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.Activities.logCtgLst, function (key, value) {
                    logsKO.ctgAlst.push(new CategorizeItem(value.cateogryName, value.logLst));
                }),

                $.each(result.LcHkActivities.logsLst, function (key, value) {
                    logsKO.LcHklst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.LcHkActivities.logSwapLst, function (key, value) {
                    logsKO.isLcHkSwpd(true);
                    logsKO.swpdLcHklst.push(new item(value.LogName, value.hasMorning, value.hasAfternoon, value.hasEvening, value.hasValue, value.MorningLst, value.AfternoonLst, value.EveningLst, value.ValusLst));
                }),
                $.each(result.LcHkActivities.logCtgLst, function (key, value) {
                    logsKO.ctgLcHklst.push(new CategorizeItem(value.cateogryName, value.logLst));
                });
            },
            error: function () {
            }
        });
    }

    return {
        load: load
    }

}();