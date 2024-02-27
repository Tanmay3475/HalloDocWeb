using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Models.DataContext;
using HalloDoc.Models;
using HalloDoc.Repository;
using HalloDoc.Repository.IRepository;

namespace HalloDocWeb.Controllers
{
    public class RequestsController : Controller
    {
        private readonly IRequestRepository _db;

        public RequestsController(IRequestRepository db)
        {
            _db = db;
        }

        // GET: Requests
        public  IActionResult Index()
        {
              return _db != null ? 
                          View(_db.GetAll()) :
                          Problem("Entity set 'ApplicationDbContext.Requests'  is null.");
        }

        // GET: Requests/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _db == null)
            {
                return NotFound();
            }

            var request =  _db.GetFirstOrDefault(m => m.Requestid == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Requestid,Requesttypeid,Userid,Firstname,Lastname,Phonenumber,Email,Status,Physicianid,Confirmationnumber,Createddate,Isdeleted,Modifieddate,Declinedby,Isurgentemailsent,Lastwellnessdate,Ismobile,Calltype,Completedbyphysician,Lastreservationdate,Accepteddate,Relationname,Casenumber,Ip,Casetag,Casetagphysician,Patientaccountid,Createduserid")] Request request)
        {
            if (ModelState.IsValid)
            {
                _db.Add(request);
                _db.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Requests/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _db == null)
            {
                return NotFound();
            }

            var request =  _db.GetFirstOrDefault(m=>m.Requestid==id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Requestid,Requesttypeid,Userid,Firstname,Lastname,Phonenumber,Email,Status,Physicianid,Confirmationnumber,Createddate,Isdeleted,Modifieddate,Declinedby,Isurgentemailsent,Lastwellnessdate,Ismobile,Calltype,Completedbyphysician,Lastreservationdate,Accepteddate,Relationname,Casenumber,Ip,Casetag,Casetagphysician,Patientaccountid,Createduserid")] Request request)
        {
            if (id != request.Requestid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(request);
                    _db.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_db.GetFirstOrDefault(M=>M.Requestid==request.Requestid)==null)
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _db == null)
            {
                return NotFound();
            }

            var request =  _db
                .GetFirstOrDefault(m => m.Requestid == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Requests'  is null.");
            }
            var request = _db.GetFirstOrDefault(m => m.Requestid == id);
            if (request != null)
            {
                _db.Remove(request);
            }
            
            _db.Save();
            return RedirectToAction(nameof(Index));
        }

        //private bool RequestExists(int id)
        //{
        //    return (bool)_db?.GetFirstOrDefault(e => e.Requestid == id));

        //}

    }
}
