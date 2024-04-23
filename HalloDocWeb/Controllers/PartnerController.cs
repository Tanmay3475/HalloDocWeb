using DocumentFormat.OpenXml.Office.Word;
using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

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
            var req = _context.Healthprofessionals.AsEnumerable().Where(m => m.Isdeleted==null).ToList();
            var req1=_context.Healthprofessionaltypes.ToList();
            var req2=_context.Regions.ToList();
            return View(new PartnerViewModel { healthprofessionals=req,Healthprofessionaltypes=req1,regions=req2});
        }
        public IActionResult AddBusiness()
        {
            var phy = _context.Healthprofessionaltypes.ToList();
            return View(new PartnerViewModel { Healthprofessionaltypes=phy});
        }public IActionResult EditBusiness(int id)
        {
            var req=_context.Healthprofessionals.FirstOrDefault(v=>v.Vendorid==id);
            var phy = _context.Healthprofessionaltypes.ToList();
            return View(new PartnerViewModel {id=req.Vendorid, Healthprofessionaltypes=phy,Name=req.Vendorname,Pro= (int)req.Profession,FaxNum=req.Faxnumber,Num=req.Phonenumber,Email=req.Email,BusinessCon=req.Businesscontact,State=req.State,Street=req.Address,City=req.City,Pin=req.Zip});
        }
        public IActionResult DeleteBusiness(int id)
        {
            var bit = new BitArray(1);
            bit[0] = true;
            var req=_context.Healthprofessionals.FirstOrDefault(v=>v.Vendorid==id);
            req.Isdeleted = bit;
            _context.Healthprofessionals.Update(req);
            _context.SaveChanges();
            return RedirectToAction("Vendor");
        }
        [HttpPost]
        public IActionResult EditBusiness(PartnerViewModel model)
        {

            var mod = _context.Healthprofessionals.FirstOrDefault(v => v.Vendorid == model.id); mod.Modifieddate = DateTime.Now; mod.Vendorname = model.Name; mod.Profession = model.Pro; mod.Faxnumber = model.FaxNum; mod.Phonenumber = model.Num; mod.Email = model.Email; mod.Businesscontact = model.BusinessCon; mod.Address = model.Street; mod.City = model.City; mod.State = model.State; mod.Zip = model.Pin ;
            _context.Healthprofessionals.Update(mod);
            _context.SaveChanges();
            return RedirectToAction("Vendor");
        }[HttpPost]
        public IActionResult AddBusiness(PartnerViewModel model)
        {
            var mod = new Healthprofessional {Createddate=DateTime.Now, Vendorname = model.Name, Profession = model.Pro, Faxnumber = model.FaxNum, Phonenumber = model.Num, Email = model.Email, Businesscontact = model.BusinessCon, Address = model.Street, City = model.City, State = model.State, Zip = model.Pin };
            _context.Healthprofessionals.Add(mod);
            _context.SaveChanges();
            return RedirectToAction("Vendor");
        }
    }
}
