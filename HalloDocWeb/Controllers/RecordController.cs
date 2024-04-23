using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace HalloDocWeb.Controllers
{
    public class RecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RecordController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult PatientRecord()
        {
            return View();
        }
        public IActionResult RecordPartial()
        {
            var req=_context.Users.ToList();
            var req1 = new List<RecordViewModel>();
            foreach(var record in req)
            {
                var req2 = new RecordViewModel { FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
req1.Add(req2);
            }
            return View(req1);
        }
    }
}
