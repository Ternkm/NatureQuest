using Microsoft.AspNetCore.Mvc;
using NatureQuest.Models;
using System.Diagnostics;

namespace NatureQuest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
