using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Models.DataContext;
using System.Globalization;
using Microsoft.AspNetCore.StaticFiles;
using System.IO.Compression;
using System.Collections;
using HalloDoc.Models;
using Microsoft.AspNetCore.Authorization;
using Services.Implementation;

namespace HalloDocWeb.Controllers
{
    [AuthorizationRepository("User")]
    public class AspnetusersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AspnetusersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aspnetusers
        public async Task<IActionResult> Index()
        {
            return _context.Aspnetusers != null ?
                        View(await _context.Aspnetusers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Aspnetusers'  is null.");
        }

        // GET: Aspnetusers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }

            var aspnetuser = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.AspNetUserId == id);
            if (aspnetuser == null)
            {
                return NotFound();
            }

            return View(aspnetuser);
        }

        // GET: Aspnetusers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aspnetusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Passwordhash,Email,Phonenumber,Createddate,Ip")] Aspnetuser aspnetuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspnetuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspnetuser);
        }

        // GET: Aspnetusers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }

            var aspnetuser = await _context.Aspnetusers.FindAsync(id);
            if (aspnetuser == null)
            {
                return NotFound();
            }
            return View(aspnetuser);
        }

        // POST: Aspnetusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Username,Passwordhash,Email,Phonenumber,Createddate,Ip")] Aspnetuser aspnetuser)
        {
            if (id != aspnetuser.AspNetUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspnetuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspnetuserExists(aspnetuser.AspNetUserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aspnetuser);
        }

        // GET: Aspnetusers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }

            var aspnetuser = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.AspNetUserId == id);
            if (aspnetuser == null)
            {
                return NotFound();
            }

            return View(aspnetuser);
        }

        // POST: Aspnetusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Aspnetusers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Aspnetusers'  is null.");
            }
            var aspnetuser = await _context.Aspnetusers.FindAsync(id);
            if (aspnetuser != null)
            {
                _context.Aspnetusers.Remove(aspnetuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: Aspnetusers/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dashboard([Bind("Id,Username,Passwordhash,Email,Phonenumber,Createddate,Ip")] Aspnetuser asp)
        {

            var aspnetuser = _context.Aspnetusers
           .FirstOrDefault(m => (m.Username == asp.Username) && (m.Passwordhash == asp.Passwordhash));
            if (aspnetuser == null)
            { return NotFound(); }
            DashboardViewModel Dashboard = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Aspnetuserid == aspnetuser.AspNetUserId);

            Dashboard.User = user;
            int id = user.Userid;

            Dashboard.Requests = (from m in _context.Requests
                                  where m.Userid == id
                                  select m).ToList();

            Dashboard.requestwisefiles = _context.Requestwisefiles.ToList();
            //DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            //                                                                                //Dashboard.birthdate = date;





            return View(Dashboard);

        }
        public IActionResult Dashboard(int id1)
        {
            var id = (int)HttpContext.Session.GetInt32("UserId");
            DashboardViewModel model = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            model.User = user;


            // var aspnetuser = await _context.Aspnetusers
            //.FirstOrDefaultAsync(m => (m.Username == asp.Username) && (m.Passwordhash == asp.Passwordhash));
            // if (aspnetuser == null)
            // { return NotFound(); }

            //DashboardViewModel Dashboard = new DashboardViewModel();
            //var user = _context.Users.FirstOrDefault(m => m.Userid == id);

            //Dashboard.User = user;
            //int id = user.Userid;
            model.Requests = (from m in _context.Requests
                              where m.Userid == id
                              select m).ToList();

            model.requestwisefiles = _context.Requestwisefiles.ToList();
            //DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            //                                                                                //Dashboard.birthdate = date;





            return View(model);

        }

        public IActionResult user_profile(int id)
        {
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            return View(new UserViewModel { city=user.City,state=user.State,dob= new DateTime(Convert.ToInt32(user.Intyear), Convert.ToInt32(user.Strmonth), Convert.ToInt32(user.Intdate)),Name=user.Firstname,last=user.Lastname,mobile=user.Mobile,Email=user.Email,street=user.Street,zip=user.Zipcode,Id=user.Userid});
        }
        [HttpPost]
        public IActionResult Submit_Profile(UserViewModel user)
        {
            var user1 = _context.Users.FirstOrDefault(m => m.Userid == user.Id);
            user1.Firstname = user.Name;
            user1.Lastname = user.last;
            user1.Email = user.Email;
            user1.City = user.city;
            user1.Street = user.street;
            user1.Mobile = user.mobile;
            user1.State = user.state;
            user1.Zipcode = user.zip;

            user1.Intdate = user.dob.Day;
            user1.Strmonth = user.dob.Month.ToString();
            user1.Intyear = user.dob.Year;
            _context.Users.Update(user1);
            _context.SaveChanges();
            return RedirectToAction("user_profile",new {id=user1.Userid});
        }
        public IActionResult view_documents(int id)
        {
            var request = _context.Requests.FirstOrDefault(m => m.Requestid == id);
            var user = _context.Users.FirstOrDefault(m => m.Userid == request.Userid);
            var aspuser = _context.Aspnetusers.FirstOrDefault(m => m.AspNetUserId == user.Aspnetuserid);
            var files = (from m in _context.Requestwisefiles
                         where m.Requestid == id
                         select m).ToList();
            DashboardViewModel ds = new DashboardViewModel { Aspuser = aspuser.AspNetUserId, User = user, requestwisefiles = files, requestid = id, Confirmation = request.Confirmationnumber };
            return View(ds);
        }

        public IActionResult Download(int id)
        {
            var path = (_context.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadAll(int id)
        {
            var checkbox = Request.Form["downloadselect"].ToList();
            var zipname = id.ToString() + "_" + DateTime.Now + ".zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in checkbox)
                    {
                        var s = Int32.Parse(item);
                        var file = await _context.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
                        var path = file.Filename;
                        var bytes = await System.IO.File.ReadAllBytesAsync(path);
                        var zipEntry = zipArchive.CreateEntry(file.Filename.Split("\\Documents\\")[1], CompressionLevel.Fastest);
                        using (var zipStream = zipEntry.Open())
                        {
                            await zipStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }
                memoryStream.Position = 0; // Reset the position
                return File(memoryStream.ToArray(), "application/zip", zipname, enableRangeProcessing: true);
            }
        }
        [HttpPost]
        public IActionResult view_documents(DashboardViewModel s)
        {

            var r = _context.Requests.FirstOrDefault(m => m.Requestid == s.requestid);
            foreach (var item in s.Filepath)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r.Requestid.ToString() + item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                _context.Add(r2);
                _context.SaveChanges();
                if (s.Filepath != null)
                {
                    uploadFile(s.Filepath, r.Requestid);
                }
            }
            return RedirectToAction("view_documents", new {id=s.requestid});
        }
        public void uploadFile(List<IFormFile> file, int id)
        {

            foreach (var item in file)
            {

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", id.ToString() + item.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    item.CopyTo(fileStream);
                }
                //Requestwisefile requestWiseFiles = new Requestwisefile
                //{
                //    Requestid = id,
                //    Filename = path,
                //    Createddate = DateTime.Now,
                //};
                //_context.Requestwisefiles.Add(requestWiseFiles);
                //_context.SaveChanges();
            }


        }
        private bool AspnetuserExists(string id)
        {
            return (_context.Aspnetusers?.Any(e => e.AspNetUserId == id)).GetValueOrDefault();
        }
        private bool AspnetuserExists(string username, string password)
        {
            return (_context.Aspnetusers?.Any(e => ((e.Username == username) && (e.Passwordhash == password)))).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public IActionResult submit()
        {
            if (Request.Form["options"] == "me")
            {
                return RedirectToAction("submit_me");
            }
            return RedirectToAction("submit_else");
        }
        public IActionResult submit_me()
        {
            var id = (int)HttpContext.Session.GetInt32("UserId");
            DashboardViewModel model = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            model.User = user;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult submit_me(DashboardViewModel model)
        {
            var id = (int)HttpContext.Session.GetInt32("UserId");
            Request r = new Request { Userid = id, Requesttypeid = 1, Status = 1, Firstname = model.FirstName, Lastname = model.LastName, Createddate = DateTime.Now, Isurgentemailsent = new BitArray(1), Confirmationnumber = model.FirstName + DateTime.Now };
            _context.Add(r);
            _context.SaveChanges();
            Requestclient r1 = new Requestclient { Requestid = r.Requestid, Firstname = model.FirstName, Lastname = model.LastName, Phonenumber = model.PhoneNumber, Address = model.Room, Notes = model.Symptoms, Email = model.Email, City = model.City, Zipcode = model.ZipCode, State = model.State, Intyear = model.DateOfBirth.Year, Strmonth = Convert.ToString(model.DateOfBirth.Month), Intdate = model.DateOfBirth.Day };
            _context.Add(r1);
            _context.SaveChanges();
            foreach (var item in model.Filepath)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r1.Requestid.ToString() + item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                _context.Add(r2);
                _context.SaveChanges();
                if (model.Filepath != null)
                {
                    uploadFile(model.Filepath, r1.Requestid);
                }
            }
            return RedirectToAction("Dashboard", id);
        }
        public IActionResult submit_else()
        {
            var id = (int)HttpContext.Session.GetInt32("UserId");
            DashboardViewModel model = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            model.User = user;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult submit_else(DashboardViewModel s)
        {
            var id = (int)HttpContext.Session.GetInt32("UserId");
            DashboardViewModel model = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            Request r = new Request { Userid = id, Requesttypeid = 2, Status = 1, Firstname = user.Firstname, Lastname = user.Lastname, Createddate = DateTime.Now, Relationname = s.Relationname, Email = user.Email, Phonenumber = user.Mobile, Isurgentemailsent = new BitArray(1), Confirmationnumber = s.FirstName + DateTime.Now };
            _context.Add(r);
            _context.SaveChanges();
            Requestclient r1 = new Requestclient { Requestid = r.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.PhoneNumber, Address = s.Room, Notes = s.Symptoms, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month), Intdate = s.DateOfBirth.Day };
            _context.Add(r1);
            _context.SaveChanges();
            foreach (var item in s.Filepath)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r.Requestid.ToString(), item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                _context.Add(r2);
                _context.SaveChanges();
                if (s.Filepath != null)
                {
                    uploadFile(s.Filepath, r1.Requestid);
                }
            }
            return RedirectToAction("Dashboard", id);
        }
    }
}
