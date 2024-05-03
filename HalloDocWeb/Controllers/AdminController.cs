using DocumentFormat.OpenXml.Drawing.Charts;
using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Runtime.InteropServices;

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
        public IActionResult Access()
        {
            var rol = applicationDb.Roles.ToList();
            return View(new CreateRoleViewModel { roles=rol});
        }
        public IActionResult ProviderLocation()
        {
            return View();
        }
        public List<MenuDataViewModel> GetRoles(int AccountType)
        {
            if (AccountType == 0)
            {
                var DataTableViewModels = from menus in applicationDb.Menus select new MenuDataViewModel { Id = menus.Menuid, Name = menus.Name };
                return DataTableViewModels.ToList();

            }
            else
            {
                var DataTableViewModels = from menus in applicationDb.Menus where menus.Accounttype == AccountType select new MenuDataViewModel { Id = menus.Menuid, Name = menus.Name };
                return DataTableViewModels.ToList();
            }
        }
        public IActionResult CreateRole()
        {
            var req=applicationDb.Menus.ToList();
            return View(new CreateRoleViewModel {menus=req});
        }
        [HttpPost]
        public IActionResult EditRole(int Id,int Acctype)
        {
            
            
            var req1 = applicationDb.Roles.FirstOrDefault(m => m.Roleid == Id);
            var req2=applicationDb.Rolemenus.AsEnumerable().Where(M=>M.Roleid == Id).ToList();

            if (Acctype == 0)
            {
                var req = applicationDb.Menus.ToList();
                return View(new CreateRoleViewModel { menus = req, name = req1.Name, accounttype = Acctype, rolemenus = req2, role_id = Id });

            }
            else
            {
                var req = applicationDb.Menus.AsEnumerable().Where(m => m.Accounttype == Acctype).ToList();
                return View(new CreateRoleViewModel { menus = req, name = req1.Name, accounttype = Acctype, rolemenus = req2, role_id = Id });

            }
        }
        
        [HttpPost]
        public IActionResult SaveRole(CreateRoleViewModel model)
        {
            var req = new Role { Accounttype = (short)model.accounttype, Name = model.name, Createdby = "Tanmay", Createddate = DateTime.Now, Isdeleted = new BitArray(1) };
            applicationDb.Roles.Add(req);
            applicationDb.SaveChanges();
            foreach (var item in model.menuid)
            {
                var rolemenu = new Rolemenu { Menuid = item, Roleid = req.Roleid };
                applicationDb.Rolemenus.Add(rolemenu);
            }
            applicationDb.SaveChanges();
            return RedirectToAction("Access");
        }
        [HttpPost]
        public IActionResult SaveEdit(CreateRoleViewModel model)
        {
            var req = applicationDb.Roles.FirstOrDefault(m => m.Roleid == model.role_id);
            req.Accounttype = (short)model.accounttype;req.Name = model.name;req.Modifiedby = "Tanmay";req.Modifieddate = DateTime.Now;
            applicationDb.Roles.Update(req);
            applicationDb.SaveChanges();
            var req1=applicationDb.Rolemenus.AsEnumerable().Where(m=>m.Roleid==req.Roleid).ToList();
            var men = new List<int>();
            foreach (var item in model.menuid)
            {
                if (req1.Any(m => m.Menuid == item))
                {
                    men.Add(item);
                }
                else
                {
                    var rolemenu = new Rolemenu { Menuid = item, Roleid = req.Roleid };
                    applicationDb.Rolemenus.Add(rolemenu);
                }
            }

            applicationDb.SaveChanges();
            foreach (var item in req1.Select(m=>m.Menuid))
            {
                if(men.Contains(item))
                {

                }
                else
                {

                    var res = applicationDb.Rolemenus.FirstOrDefault(M => M.Menuid == item && M.Roleid == model.role_id);
                    applicationDb.Rolemenus.Remove(res);
                }
            }
            applicationDb.SaveChanges();
            return RedirectToAction("Access");
        }
        [HttpPost]
        public IActionResult DeleteRole(int Id)
        {
            var req = applicationDb.Roles.FirstOrDefault(m=>m.Roleid==Id);
            req.Isdeleted[0] = true;
                applicationDb.Roles.Update(req);
            applicationDb.SaveChanges();
            return RedirectToAction("Access");
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
            var profile = new MyProfileViewModel { Address1 = admin.Address1, Address2 = admin.Address2, Email = admin.Email, password = aspnet.Passwordhash, state = region1.Name, Phone = admin.Mobile, UserName = aspnet.Username, FirstName = admin.Firstname, LastName = admin.Lastname, ZipCode = admin.Zip, City = admin.City,altphone=admin.Altphone };
            //List<Region> regions = new List<Region>();
            var x = new List<Region>();
            foreach (var item in region)
            {
                x.AddRange(applicationDb.Regions.ToList().Where(m => m.Regionid == item.Regionid));
                //var regis = applicationDb.Regions.FirstOrDefault(m => m.Regionid == item.Regionid);
            }
            var y=applicationDb.Regions.AsEnumerable().Except(x).ToList();
                profile.regions = x;
            profile.regionexcept= y;
            var z=applicationDb.Regions.ToList();
            profile.All= z;
            return View(profile);
        }
        [HttpPost]
        public IActionResult MyProfile(string pass)
        {
            var id = HttpContext.Session.GetInt32("Admin_Id");
            var admin = applicationDb.Admins.FirstOrDefault(a => a.Adminid == id);
            var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            aspnet.Passwordhash = pass;
            applicationDb.Aspnetusers.Update(aspnet);
            applicationDb.SaveChanges();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("MyProfile");
        }        
        [HttpPost]
        public IActionResult EditProfile(MyProfileViewModel model)
        {
            var id = HttpContext.Session.GetInt32("Admin_Id");
            var admin = applicationDb.Admins.FirstOrDefault(a => a.Adminid == id);
            //var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            //aspnet.Passwordhash = pass;
            //applicationDb.Aspnetusers.Update(aspnet);
            admin.Firstname=model.FirstName; admin.Lastname=model.LastName; admin.Email=model.Email; admin.Mobile = model.Phone;
            admin.Modifieddate=DateTime.Now;
            applicationDb.Admins.Update(admin);
            applicationDb.SaveChanges();
            List<Region> sel = new List<Region>();
            if ( model.selected!=null)
            {
                foreach (var item in model.selected)
                {
                    var reg = applicationDb.Regions.FirstOrDefault(m => m.Regionid == item);
                    sel.Add(reg);
                }
            }
            List<Region> notsel = new List<Region>();
            if (model.notselected!=null)
            {
                foreach (var item in model.notselected)
                {
                    var reg = applicationDb.Regions.FirstOrDefault(m => m.Regionid == item);
                    notsel.Add(reg);
                }
            }
            var reg1 = applicationDb.Regions.AsEnumerable().Except(sel).ToList();
            var reg2 = applicationDb.Regions.AsEnumerable().Except(notsel).ToList();
            foreach (var item in reg1) {
                var reg = applicationDb.Adminregions.FirstOrDefault(M => M.Regionid == item.Regionid);
                    if (reg != null)
                {
                    applicationDb.Adminregions.Remove(reg);
                    applicationDb.SaveChanges();
                }
            }
            foreach(var item in reg2) {
                var reg3 = applicationDb.Adminregions.FirstOrDefault(m => m.Regionid == item.Regionid && m.Adminid == admin.Adminid);
                if (reg3 == null)
                {
                    var reg = new Adminregion { Regionid = item.Regionid, Adminid = admin.Adminid };
                    applicationDb.Adminregions.Add(reg);
                    applicationDb.SaveChanges();
                }
            }
            //var reg2 = applicationDb.Regions.AsEnumerable().Except(notsel).ToList();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("MyProfile");
        }
        [HttpPost]
        public IActionResult EditShipProfile(MyProfileViewModel model)
        {
            var id = HttpContext.Session.GetInt32("Admin_Id");
            var admin = applicationDb.Admins.FirstOrDefault(a => a.Adminid == id);
            //var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            //aspnet.Passwordhash = pass;
            //applicationDb.Aspnetusers.Update(aspnet);
            admin.Address1=model.Address1; admin.Address2=model.Address2; admin.City=model.City; admin.Altphone = model.altphone;admin.Regionid = model.regid;
            admin.Zip = model.ZipCode;
            admin.Modifieddate=DateTime.Now;
            applicationDb.Admins.Update(admin);
            applicationDb.SaveChanges();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("MyProfile");
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
                    if(admin==null)
                    {
                        TempData["user"] = "User does not exist.";
                        TempData["pass"] = "Password is not valid";
                        return View();
                    }
                    int id = admin.Adminid;
                    HttpContext.Session.SetInt32("Admin_Id", id);
                    HttpContext.Session.SetString("Admin_User", admin.Firstname);
                    return RedirectToAction("tabs", "AdminStatus");
                }
                var asp = _db1.GetFirstOrDefault(m => m.Username == a.Username);
                if (asp == null) {
                    TempData["user"] = "User does not exist.";
                    return View();
                }
                else
                {
                    TempData["pass"] = "Password is not valid";
                    return View();
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
