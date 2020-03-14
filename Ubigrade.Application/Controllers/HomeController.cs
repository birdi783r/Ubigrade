using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ubigrade.Application.Models;
using Ubigrade.Library.Google;
using static Ubigrade.Library.Google.GetGoogleData;


namespace Ubigrade.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("getclasses")]
        public async Task<IActionResult> GetClasses()
        {
            var userFromManager = await _userManager.GetUserAsync(User);

            UserCredential credential = GoogleProviderHelper.CreateUserCredential(User);
            var x = await GetAllStudentActiveCourses(credential);
            var y = JsonConvert.SerializeObject(x);
            return Content(y);
        }
        [Route("getstudents")]
        public async Task<IActionResult> GetStudentsOfCourse()
        {
            var userFromManager = await _userManager.GetUserAsync(User);
            //von modo fr lorenz kurs
            string courseid = "59971393925";
            //von ubigrade19-20 email
            string courseid2 = "59969666110";
            UserCredential credential = GoogleProviderHelper.CreateUserCredential(User);
            var x = await GetAllStudentsOfCourse(credential, courseid2);
            var y = JsonConvert.SerializeObject(x);
            return Content(y);
        }
        [Route("getstudentdata")]
        public async Task<IActionResult> GetStudentData()
        {
            var userFromManager = await _userManager.GetUserAsync(User);
            //von nikola zdravkovic
            //aus einem kurs von ubigrade19-20@ mail
            string userid = "115978104850547008611";
            //von ubigrade19-20 email kursid 
            string courseid2 = "59969666110";
            UserCredential credential = GoogleProviderHelper.CreateUserCredential(User);
            var x = await GetStudentDataByUserId(credential, courseid2, userid);
            var y = JsonConvert.SerializeObject(x);
            return Content(y);
        }
        [Route("getProfile")]
        public async Task<IActionResult> GetStudentData2()
        {
            var userFromManager = await _userManager.GetUserAsync(User);
            //von nikola zdravkovic
            string userid = "115978104850547008611";
            string userid2 = "ubigrade19-20@htlwienwest.at";
            UserCredential credential = GoogleProviderHelper.CreateUserCredential(User);
            var x = await GetClassroomUserProfile(credential, userid2);
            var y = JsonConvert.SerializeObject(x);
            return Content(y);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

