using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace HalloDocWeb.Controllers
{
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PartnerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Vendor()
        {
            
            return View();
        }
        public IActionResult AddBusiness()
        {
            var phy = _context.Healthprofessionaltypes.ToList();
            return View(new PartnerViewModel { Healthprofessionaltypes=phy});
        }
        [HttpPost]
        public IActionResult AddBusiness(PartnerViewModel model)
        {
            var mod = new Healthprofessional {Createddate=DateTime.Now, Vendorname = model.Name, Profession = model.Pro, Faxnumber = model.FaxNum, Phonenumber = model.Num, Email = model.Email, Businesscontact = model.BusinessCon, Address = model.Street, City = model.City, State = model.State, Zip = model.Pin };
            _context.Healthprofessionals.Add(mod);
            _context.SaveChanges();
            return RedirectToAction("Vendor");
        }
    }
}
