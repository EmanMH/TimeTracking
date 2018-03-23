using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{

    public class LogsModel
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string ServiceCode { get; set; }
        public List<int> selectedLogs = new List<int>();
        public DailyLivingActivities DLActivities = new DailyLivingActivities();
        public FoodPrepActivities FPActivities = new FoodPrepActivities();
        public Activities Activities = new Activities();
        public LightChoresHouseKeeping LcHk = new LightChoresHouseKeeping();
    }

    public class DailyLivingActivities
    {
        public List<int> ShowerBathingMorninglst = new List<int>();
        public List<int> ShowerBathingAfternoonlst = new List<int>();
        public List<int> ShowerBathingEveninglst = new List<int>();

        public List<int> OralHygeineMorninglst = new List<int>();
        public List<int> OralHygeineAfternoonlst = new List<int>();
        public List<int> OralHygeineEveninglst = new List<int>();

        public List<int> DressingMorninglst = new List<int>();
        public List<int> DressingAfternoonlst = new List<int>();
        public List<int> DressingEveninglst = new List<int>();

        public List<int> HairCutMorninglst = new List<int>();
        public List<int> HairCutAfternoonlst = new List<int>();
        public List<int> HairCutEveninglst = new List<int>();

    }

    public class FoodPrepActivities
    {
        public List<int> MealPrepMorninglst = new List<int>();
        public List<int> MealPrepAfternoonlst = new List<int>();
        public List<int> MealPrepEveninglst = new List<int>();

        public List<int> SnacksMorninglst = new List<int>();
        public List<int> SnacksAfternoonlst = new List<int>();
        public List<int> SnacksEveninglst = new List<int>();

        public List<int> DrinksMorninglst = new List<int>();
        public List<int> DrinksAfternoonlst = new List<int>();
        public List<int> DrinksEveninglst = new List<int>();
        
    }

    public class Activities
    {
        public List<int> Eventslst = new List<int>();
        public List<int> WeighInPublixlst = new List<int>();

        public List<int> Ipadlst = new List<int>();
        public List<int> Readinglst = new List<int>();
        public List<int> Exerciselst = new List<int>();
        public List<int> DVDTVlst = new List<int>();
    }

    public class LightChoresHouseKeeping
    {
        public List<int> CleaningSpillsMopMorninglst = new List<int>();
        public List<int> CleaningSpillsMopAfternoonlst = new List<int>();
        public List<int> CleaningSpillsMopEveninglst = new List<int>();

        public List<int> ChangeBedLinensMorninglst = new List<int>();
        public List<int> ChangeBedLinensAfternoonlst = new List<int>();
        public List<int> ChangeBedLinensEveninglst = new List<int>();

        public List<int> LaundryMorninglst = new List<int>();
        public List<int> LaundryAfternoonlst = new List<int>();
        public List<int> LaundryEveninglst = new List<int>();

        public List<int> CleanRoomlst = new List<int>();

        public List<int> CleanPathRoomlst = new List<int>();

    }

}