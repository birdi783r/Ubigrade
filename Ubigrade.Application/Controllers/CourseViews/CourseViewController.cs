using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ubigrade.Application.Models.Course;
using Ubigrade.Library.Google;
using static Ubigrade.Library.Google.GetGoogleData;


namespace Ubigrade.Application.Controllers.CourseViews
{
    public class CourseViewController : Controller
    {
        // GET: CourseView
        private readonly ILogger<CourseViewController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CourseViewController(ILogger<CourseViewController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ActionResult> IndexAsync()
        {
            var userFromManager = await _userManager.GetUserAsync(User);
            UserCredential credential = GoogleProviderHelper.CreateUserCredential(User);
            var x = await GetAllStudentActiveCourses(credential);
            var y = JsonConvert.SerializeObject(x);
            var model = new List<CourseViewModel>();
            foreach (var c in x)
            {
                model.Add(new CourseViewModel
                {

                });
            }
            return View(y);
        }

        // GET: CourseView/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CourseView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseView/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CourseView/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: CourseView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CourseView/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}