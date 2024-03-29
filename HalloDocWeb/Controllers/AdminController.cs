using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HalloDocWeb.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDb;
        private readonly IAdminRepository _db;
        private readonly IAspnetuserRepository _db1;
        public AdminController(IAdminRepository db, IAspnetuserRepository db1, ApplicationDbContext applicationDb)
        {
            this.applicationDb = applicationDb;
            _db = db;
            _db1 = db1;
        }
        public IActionResult Admin_Login()
        {
            return View();
        } 
        public IActionResult CreateRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRequest(CreateRequestViewModel s)
        {
            Request r1 = new Request { Userid=1,Requesttypeid = 5, Status = 1, Firstname = s.FirstName, Lastname = s.LastName, Createddate = DateTime.Now, Email = s.Email, Phonenumber = s.Phone, Isurgentemailsent = new BitArray(1), Confirmationnumber = DateTime.Now.ToString() };
            applicationDb.Add(r1);
            applicationDb.SaveChanges();
            Requestclient r2 = new Requestclient { Requestid = r1.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.Phone, Address = s.Room, Notes = s.AdminNotes, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month), Intdate = s.DateOfBirth.Day };
            applicationDb.Add(r2);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
        }
       


        public IActionResult MyProfile()
        {
            var id = HttpContext.Session.GetInt32("Admin_Id");
            var admin = applicationDb.Admins.FirstOrDefault(a => a.Adminid == id);
            var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regionid;
            var region = applicationDb.Adminregions.ToList().Where(m => m.Adminid == id);
            var region1 = applicationDb.Regions.FirstOrDefault(a => a.Regionid == admin.Regionid);
            var profile = new MyProfileViewModel { Address1 = admin.Address1, Address2 = admin.Address2, Email = admin.Email, password = aspnet.Passwordhash, state = region1.Name, Phone = admin.Mobile, UserName = aspnet.Username, FirstName = admin.Firstname, LastName = admin.Lastname, ZipCode = admin.Zip, City = admin.City };
            //List<Region> regions = new List<Region>();
            var x = new List<Region>();
            foreach (var item in region)
            {
                x.AddRange(applicationDb.Regions.ToList().Where(m => m.Regionid == item.Regionid));
                //var regis = applicationDb.Regions.FirstOrDefault(m => m.Regionid == item.Regionid);
            }
                profile.regions = x;
            
            return View(profile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Admin_Login(AdminLoginViewModel a)
        {
            if (ModelState.IsValid)
            {
                var aspnetuser = _db1.GetFirstOrDefault(m => m.Username == a.Username && m.Passwordhash == a.Password);
                if (aspnetuser != null)
                {
                    var admin = _db.GetFirstOrDefault(m => m.Aspnetuserid == aspnetuser.AspNetUserId);
                    int id = admin.Adminid;
                    HttpContext.Session.SetInt32("Admin_Id", id);
                    HttpContext.Session.SetString("Admin_User", admin.Firstname);
                    return RedirectToAction("tabs", "AdminStatus");
                }
            }
            return NotFound();
        }
        public IActionResult Admin_Reset()
        {
            return View();
        }

    }
}
