using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeTracking.Controllers
{
    public class StaticController : Controller
    {
        // GET: Static
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Web()
        {
            return View();
        }

        public ActionResult CrystalReports()
        {
            return View();
        }

        public ActionResult QA()
        {
            return View();
        }
        public ActionResult WMBECertifications()
        {
            return View();
        }
        public ActionResult Jobs()
        {
            return View();
        }
    }
}