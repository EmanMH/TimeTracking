using Business.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Models;
using Utilities;

namespace TimeTracking.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser()
        {
           
                return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminServices  adminServices = new AdminServices();
                RandomGenerator rg = new RandomGenerator();
                var password = "Password@123";

                var UserManager= HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var username = model.FirstName[0] + model.LastName + rg.GenerateRandomNo();

                var usere = adminServices.getByEmail(model.Email);
                if (usere != null)
                {
                    ModelState.AddModelError("", "Email already exists!");
                    return View(model);
                }

                while (adminServices.getByUsername(username) != null)
                {
                    username = model.FirstName[0] + model.LastName + rg.GenerateRandomNo();
                }

                var user = new ApplicationUser { UserName = username, Email = model.Email };
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    adminServices.AddUserInfo(model.FirstName, model.LastName, model.MiddleName, user.Id, true);

                    return RedirectToAction("username", "Home", new { username = username, password = password });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View( model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ReviewTimesheet()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getAdminSheets()
        {
            AdminServices adminServices = new AdminServices();
            var timesheets = adminServices.getAllSheets();
            SheetReviewmodel sr = new SheetReviewmodel();
            var pending = timesheets.Where(s => s.fk_statusid == 1).ToList();
            var Approved = timesheets.Where(s => s.fk_statusid == 2).ToList();
            var Rejected = timesheets.Where(s => s.fk_statusid == 3).ToList();
            return Json(null);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
