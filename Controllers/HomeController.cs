using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BE.Controllers
{
    public class HomeController : Controller
    {
        private readonly SurveyProjectContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(SurveyProjectContext context)
        {
            _context = context;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            var counts = new { UserCount = _context.Users.Count(x => x.Role == "Staff" || x.Role == "Student"), AdminCount = _context.Users.Count(x => x.Role == "Admin"), PendingCount = _context.Users.Count(y => y.Active == 0), SurveyCount = _context.Surveys.Count(), QuestionCount = _context.Questions.Count() };
            return View(counts);
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