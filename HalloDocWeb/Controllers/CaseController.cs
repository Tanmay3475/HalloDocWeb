using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HalloDocWeb.Controllers
{
    
    public class CaseController : Controller
    {
        private readonly ApplicationDbContext applicationDb;
        public CaseController(ApplicationDbContext applicationDb)
        {
               this.applicationDb = applicationDb;
        }
        public IActionResult ViewCase(int id)
        {
            var req=applicationDb.Requests.FirstOrDefault(req=>req.Requestid==id);
            var req1 = applicationDb.Requestclients.FirstOrDefault(req=>req.Requestid==id);
            var reg = applicationDb.Regions.FirstOrDefault(r=>r.Regionid==req1.Regionid);
            return View(new ViewCaseViewModel { Address=req1.Street,PhoneNumber=req.Phonenumber,Confirmation=req.Confirmationnumber,DateOfBirth= new DateTime(Convert.ToInt32(req1.Intyear), Convert.ToInt32(req1.Strmonth), Convert.ToInt32(req1.Intdate)) ,Region=reg.Name,Email=req.Email,LastName=req.Lastname,FirstName=req.Firstname,Room=req1.Address,Symptoms=req1.Notes});
        }
    }
}
