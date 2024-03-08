using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HalloDoc.Repository;
using HalloDoc.Repository.IRepository;
namespace HalloDocWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService; 
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,IJwtService jwtService,IUserService userService)
        {
            _logger = logger;
            _context = context;
            _jwtService = jwtService;
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([Bind("Id,Username,Passwordhash,Email,Phonenumber,Createddate,Ip")] Aspnetuser asp)
        {
            var usert = _userService.Login(asp.Username, asp.Passwordhash);
            //var user1 = new AuthManager().
            if (usert == null)
            {
                return View();
            }
            else
            {
                var aspnetuser = _context.Aspnetusers
               .FirstOrDefault(m => (m.Username == asp.Username) && (m.Passwordhash == asp.Passwordhash));
                if (aspnetuser == null)
                { return NotFound(); }
                else
                {
                    var jwtToken = _jwtService.GenerateJwtToken(usert);
                    Response.Cookies.Append("jwt", jwtToken);
                    DashboardViewModel Dashboard = new DashboardViewModel();
                    var user = _context.Users.FirstOrDefault(m => m.Aspnetuserid == aspnetuser.AspNetUserId);

                    Dashboard.User = user;
                    int id = user.Userid;
                    HttpContext.Session.SetInt32("UserId", id);
                    Dashboard.Requests = (from m in _context.Requests
                                          where m.Userid == id
                                          select m).ToList();

                    Dashboard.requestwisefiles = _context.Requestwisefiles.ToList();
                    //DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
                    //                                                                                //Dashboard.birthdate = date;





                    return RedirectToAction("Dashboard", "Aspnetusers");
                }
            }
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
