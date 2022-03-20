using EMTrackerDev.Data;
using EMTrackerDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EMTrackerDev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EMTrackerDevContext _context;
        public HomeController(EMTrackerDevContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

    

        public IActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            Console.WriteLine("USER5 " + userId);
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string u, string p)
        {
            var users = _context.Users.ToList();
            for(int i = 0; i< users.Count; i++)
            {
                if (users[i].UserName.Equals(u) && users[i].Password.Equals(p))
                {
                    User logged = users[i];
                    int id = logged.UserId;
                    return RedirectToAction("Index", new {userId=id});

                }

            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
