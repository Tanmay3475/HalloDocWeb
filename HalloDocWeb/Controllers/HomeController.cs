using HalloDocWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDocWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Forgot()
        {
            return View();
        }
        public IActionResult Submit_request()
        {
            return View();
        }
      
        public IActionResult Business_request()
        {
            return View();
        }
        public IActionResult Request_screen()
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
