using FastReport.Web;
using MCB.VBO.Microservices.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.VBO.Microservices.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private string _dbPath { get; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbPath = AppContext.BaseDirectory;
        }

        public IActionResult Index()
        {
            var webReport = new WebReport();
            string reportPath = Path.Combine(_dbPath, "Statement.frx");
            webReport.Report.Load(reportPath);

            return View(webReport);
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
