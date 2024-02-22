using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDocWeb.DataContext;
using HalloDocWeb.DataModels;
using HalloDocWeb.ViewModels;
using System.Globalization;

namespace HalloDocWeb.Controllers
{
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
        public async Task<IActionResult> Dashboard([Bind("Id,Username,Passwordhash,Email,Phonenumber,Createddate,Ip")] Aspnetuser asp)
        {
            var aspnetuser = await _context.Aspnetusers
           .FirstOrDefaultAsync(m => (m.Username == asp.Username) && (m.Passwordhash == asp.Passwordhash));
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
        public  IActionResult Dashboard(int id)
        {
           // var aspnetuser = await _context.Aspnetusers
           //.FirstOrDefaultAsync(m => (m.Username == asp.Username) && (m.Passwordhash == asp.Passwordhash));
           // if (aspnetuser == null)
           // { return NotFound(); }
            DashboardViewModel Dashboard = new DashboardViewModel();
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);

            Dashboard.User = user;
            //int id = user.Userid;
            Dashboard.Requests = (from m in _context.Requests
                                  where m.Userid == id
                                  select m).ToList();
            
            Dashboard.requestwisefiles = _context.Requestwisefiles.ToList();
            //DateTime date = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate));
            //                                                                                //Dashboard.birthdate = date;





            return View(Dashboard);

        }
        public IActionResult user_profile(int id)
        {
            var user = _context.Users.FirstOrDefault(m => m.Userid == id);
            return View(user);
        }
        public IActionResult view_documents(int id)
        {
            
            var request =  _context.Requests.FirstOrDefault(m => m.Requestid == id);
            var user = _context.Users.FirstOrDefault(m => m.Userid == request.Userid);
            var aspuser= _context.Aspnetusers.FirstOrDefault(m => m.AspNetUserId == user.Aspnetuserid);
            return View();
        }
        private bool AspnetuserExists(string id)
        {
            return (_context.Aspnetusers?.Any(e => e.AspNetUserId == id)).GetValueOrDefault();
        }
        private bool AspnetuserExists(string username, string password)
        {
            return (_context.Aspnetusers?.Any(e => ((e.Username == username) && (e.Passwordhash == password)))).GetValueOrDefault();
        }
        
    }
}
