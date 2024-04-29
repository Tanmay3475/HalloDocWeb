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
        public IActionResult SearchRecord()
        {
            return View();
        }
        public IActionResult Explore(int Id)
        {
            var req = _context.Requests.AsEnumerable().Where(m => m.Userid == Id).ToList();
            var req1 = new List<ExploreViewModel>();
            foreach(var re in req)
            {
                var r = new ExploreViewModel { Name=re.Firstname,CreatedDate=new DateOnly(re.Createddate.Year,re.Createddate.Month,re.Createddate.Day),Confirmation=re.Confirmationnumber,status=re.Status};
                var res = _context.Requeststatuslogs.AsEnumerable().FirstOrDefault(m => m.Status == 6 && m.Requestid == re.Requestid);
                if(res!=null)
                {
                    r.ConcludedDate = new DateOnly(res.Createddate.Year,res.Createddate.Month,res.Createddate.Day);
                }
                if (re.Physicianid != null)
                {
                    var phy = _context.Physicians.FirstOrDefault(m => m.Physicianid == re.Physicianid);
                    r.Pro = phy.Firstname;
                }
                req1.Add(r);
            }
            return View(req1);
        }
        public IActionResult RecordPartial(RecordViewModel model)
        {
if(model.FirstName==null && model.LastName==null &&  model.Email==null&& model.Phone==null)
            {
                var req = _context.Users.ToList();
                var req1 = new List<RecordViewModel>();
                foreach (var record in req)
                {
                    var req2 = new RecordViewModel {Id=record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
                    req1.Add(req2);
                }
                return View(req1);
            }
            else
            {
                var req = _context.Users.Where(m=>m.Email.ToLower().Contains(model.Email.ToLower())&&m.Firstname.ToLower().Contains(model.FirstName.ToLower())&&m.Lastname.ToLower().Contains(model.LastName.ToLower())&&m.Mobile.Contains(model.Phone)).ToList();
                var req1 = new List<RecordViewModel>();
                foreach (var record in req)
                {
                    var req2 = new RecordViewModel {Id=record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
                    req1.Add(req2);
                }
                return View(req1);

            }
        }
//        public IActionResult SearchPartial()
//        {
//if(model.FirstName==null && model.LastName==null &&  model.Email==null&& model.Phone==null)
//            {
//                var req = _context.Users.ToList();
//                var req1 = new List<RecordViewModel>();
//                foreach (var record in req)
//                {
//                    var req2 = new RecordViewModel {Id=record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
//                    req1.Add(req2);
//                }
//                return View(req1);
//            }
//            else
//            {
//                var req = _context.Users.Where(m=>m.Email.ToLower().Contains(model.Email.ToLower())&&m.Firstname.ToLower().Contains(model.FirstName.ToLower())&&m.Lastname.ToLower().Contains(model.LastName.ToLower())&&m.Mobile.Contains(model.Phone)).ToList();
//                var req1 = new List<RecordViewModel>();
//                foreach (var record in req)
//                {
//                    var req2 = new RecordViewModel {Id=record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
//                    req1.Add(req2);
//                }
//                return View(req1);

//            }
//        }
    }
}
