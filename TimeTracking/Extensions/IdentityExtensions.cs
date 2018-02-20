using Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace TimeTracking.Extensions
{
    public static class IE
    {
        public static string Getusersname()
        {
            EmployeeServices ad = new EmployeeServices();
            var username = HttpContext.Current.User.Identity.Name;
            // Test for null to avoid issues during local testing
            return ad.getUsername(username);
        }

        //public static string getName()
        //{
        //    EmployeeServices ad = new EmployeeServices();
        //    var username = HttpContext.Current.User.Identity.Name;
        //    return ad.getUsername(username);
        //}
    }
}