Logs = function () {
    var logsKO;

    var logsViewModel = function () {
        var self = this;

        //Daily Activities
        self.isSBMLst = ko.observableArray([]);
        self.isSBALst = ko.observableArray([]);
        self.isSBELst = ko.observableArray([]);

        self.isOHMLst = ko.observableArray([]);
        self.isOHALst = ko.observableArray([]);
        self.isOHELst = ko.observableArray([]);

        self.isDMLst = ko.observableArray([]);
        self.isDALst = ko.observableArray([]);
        self.isDELst = ko.observableArray([]);

        self.isHCMLst = ko.observableArray([]);
        self.isHCALst = ko.observableArray([]);
        self.isHCELst = ko.observableArray([]);

        self.isMPMLst = ko.observableArray([]);
        self.isMPALst = ko.observableArray([]);
        self.isMPELst = ko.observableArray([]);

        self.isSMLst = ko.observableArray([]);
        self.isSALst = ko.observableArray([]);
        self.isSELst = ko.observableArray([]);

        self.isDrnkMLst = ko.observableArray([]);
        self.isDrnkALst = ko.observableArray([]);
        self.isDrnkELst = ko.observableArray([]);

        //Activites
        self.isELst = ko.observableArray([]);

        self.isWIPLst = ko.observableArray([]);

        self.isIPLst = ko.observableArray([]);

        self.isRLst = ko.observableArray([]);

        self.isExLst = ko.observableArray([]);

        self.isDTLst = ko.observableArray([]);

        //Light Chores/HouseKeeping
        self.isCSMMLst = ko.observableArray([]);
        self.isCSMALst = ko.observableArray([]);
        self.isCSMELst = ko.observableArray([]);

        self.isCBLMLst = ko.observableArray([]);
        self.isCBLALst = ko.observableArray([]);
        self.isCBLELst = ko.observableArray([]);

        self.islMLst = ko.observableArray([]);
        self.islALst = ko.observableArray([]);
        self.islELst = ko.observableArray([]);

        self.isCRLst = ko.observableArray([]);

        self.isCBLst = ko.observableArray([]);
        
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
            
            alert(ko.toJSON(self.isChecked()));
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

                    //Daily Activities
                    $.each(result.DLActivities.ShowerBathingMorninglst, function (key, value) {
                        logsKO.isSBMLst.push(value);
                    }),
                    $.each(result.DLActivities.ShowerBathingAfternoonlst, function (key, value) {
                        logsKO.isSBALst.push(value);
                    }),
                    $.each(result.DLActivities.ShowerBathingEveninglst, function (key, value) {
                        logsKO.isSBELst.push(value);
                    }),

                    $.each(result.DLActivities.OralHygeineMorninglst, function (key, value) {
                        logsKO.isOHMLst.push(value);
                    }),
                    $.each(result.DLActivities.OralHygeineAfternoonlst, function (key, value) {
                        logsKO.isOHALst.push(value);
                    }),
                    $.each(result.DLActivities.OralHygeineEveninglst, function (key, value) {
                        logsKO.isOHELst.push(value);
                    }),


                    $.each(result.DLActivities.DressingMorninglst, function (key, value) {
                        logsKO.isDMLst.push(value);
                    }),
                    $.each(result.DLActivities.DressingAfternoonlst, function (key, value) {
                        logsKO.isDALst.push(value);
                    }),
                    $.each(result.DLActivities.DressingEveninglst, function (key, value) {
                        logsKO.isDELst.push(value);
                    }),


                    $.each(result.DLActivities.HairCutMorninglst, function (key, value) {
                        logsKO.isHCMLst.push(value);
                    }),
                    $.each(result.DLActivities.HairCutAfternoonlst, function (key, value) {
                        logsKO.isHCALst.push(value);
                    }),
                    $.each(result.DLActivities.HairCutEveninglst, function (key, value) {
                        logsKO.isHCELst.push(value);
                    }),

                    //Food Prep
                    $.each(result.FPActivities.MealPrepMorninglst, function (key, value) {
                        logsKO.isMPMLst.push(value);
                    }),
                    $.each(result.FPActivities.MealPrepAfternoonlst, function (key, value) {
                        logsKO.isMPALst.push(value);
                    }),
                    $.each(result.FPActivities.MealPrepEveninglst, function (key, value) {
                        logsKO.isMPELst.push(value);
                    }),

                    $.each(result.FPActivities.SnacksMorninglst, function (key, value) {
                        logsKO.isSMLst.push(value);
                    }),
                    $.each(result.FPActivities.SnacksAfternoonlst, function (key, value) {
                        logsKO.isSALst.push(value);
                    }),
                    $.each(result.FPActivities.SnacksEveninglst, function (key, value) {
                        logsKO.isSELst.push(value);
                    }),


                    $.each(result.FPActivities.DrinksMorninglst, function (key, value) {
                        logsKO.isDrnkMLst.push(value);
                    }),
                    $.each(result.FPActivities.DrinksAfternoonlst, function (key, value) {
                        logsKO.isDrnkALst.push(value);
                    }),
                    $.each(result.FPActivities.DrinksEveninglst, function (key, value) {
                        logsKO.isDrnkELst.push(value);
                    }),

                    //Activities
                    $.each(result.Activities.Eventslst, function (key, value) {
                        logsKO.isELst.push(value);
                    }),

                    $.each(result.Activities.WeighInPublixlst, function (key, value) {
                        logsKO.isWIPLst.push(value);
                    }),

                    $.each(result.Activities.Ipadlst, function (key, value) {
                        logsKO.isIPLst.push(value);
                    }),

                    $.each(result.Activities.Readinglst, function (key, value) {
                        logsKO.isRLst.push(value);
                    }),

                    $.each(result.Activities.Exerciselst, function (key, value) {
                        logsKO.isExLst.push(value);
                    }),

                    $.each(result.Activities.DVDTVlst, function (key, value) {
                        logsKO.isDTLst.push(value);
                    }),

                    //Light Chores/HouseKeeping
                    $.each(result.LcHk.CleaningSpillsMopMorninglst, function (key, value) {
                        logsKO.isCSMMLst.push(value);
                    }),
                    $.each(result.LcHk.CleaningSpillsMopAfternoonlst, function (key, value) {
                        logsKO.isCSMALst.push(value);
                    }),
                    $.each(result.LcHk.CleaningSpillsMopEveninglst, function (key, value) {
                        logsKO.isCSMELst.push(value);
                    }),

                    $.each(result.LcHk.ChangeBedLinensMorninglst, function (key, value) {
                        logsKO.isCBLMLst.push(value);
                    }),
                    $.each(result.LcHk.ChangeBedLinensAfternoonlst, function (key, value) {
                        logsKO.isCBLALst.push(value);
                    }),
                    $.each(result.LcHk.ChangeBedLinensEveninglst, function (key, value) {
                        logsKO.isCBLELst.push(value);
                    }),

                    $.each(result.LcHk.LaundryMorninglst, function (key, value) {
                        logsKO.islMLst.push(value);
                    }),
                    $.each(result.LcHk.LaundryAfternoonlst, function (key, value) {
                        logsKO.islALst.push(value);
                    }),
                    $.each(result.LcHk.LaundryEveninglst, function (key, value) {
                        logsKO.islELst.push(value);
                    }),

                    $.each(result.LcHk.CleanRoomlst, function (key, value) {
                        logsKO.isCRLst.push(value);
                    }),

                    $.each(result.LcHk.CleanPathRoomlst, function (key, value) {
                        logsKO.isCBLst.push(value);
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