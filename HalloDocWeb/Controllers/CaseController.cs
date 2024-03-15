using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HalloDocWeb.Controllers
{

    public class CaseController : Controller
    {
        private readonly IViewNotes viewNotes;
        private readonly ApplicationDbContext applicationDb;
        public CaseController(ApplicationDbContext applicationDb, IViewNotes viewNotes)
        {
            this.applicationDb = applicationDb;
            this.viewNotes = viewNotes;
        }
        public IActionResult ViewCase(int id)
        {
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == id);
            var req1 = applicationDb.Requestclients.FirstOrDefault(req => req.Requestid == id);
            var reg = applicationDb.Regions.FirstOrDefault(r => r.Regionid == req1.Regionid);
            return View(new ViewCaseViewModel { Address = req1.Street, PhoneNumber = req.Phonenumber, Confirmation = req.Confirmationnumber, DateOfBirth = new DateTime(Convert.ToInt32(req1.Intyear), Convert.ToInt32(req1.Strmonth), Convert.ToInt32(req1.Intdate)), Region = reg.Name, Email = req.Email, LastName = req.Lastname, FirstName = req.Firstname, Room = req1.Address, Symptoms = req1.Notes });
        }
        public IActionResult ViewUploads(int id)
        {
            var request = applicationDb.Requests.FirstOrDefault(m => m.Requestid == id);
            var files = (from m in applicationDb.Requestwisefiles
                         where m.Requestid == id
                         select m).ToList();
            DashboardViewModel ds = new DashboardViewModel {FirstName=request.Firstname, requestwisefiles = files, requestid = id, Confirmation = request.Confirmationnumber };
            return View(ds);
        }
        public IActionResult ViewNotes(int id)
        {
            //var model = new ViewNotesViewModel();
            //var request = applicationDb.Requests.FirstOrDefault(m => m.Requestid==id);
            //var adminName = "hello";
            //var physicianName = "Dr.Agola";
            //model.adminName = adminName;
            //model.PhysicianName = physicianName;
            //model.createddate = DateTime.Now;
            //request.Status = 6;
            //applicationDb.Requests.Update(request);

            var req = applicationDb.Requestnotes.FirstOrDefault(req => req.Requestid == id);
            var req1 = applicationDb.Requeststatuslogs.FirstOrDefault(r => r.Requestid == id);
            if (req1 != null)
            {
                var req2 = applicationDb.Physicians.FirstOrDefault(r => r.Physicianid == req1.Transtophysicianid);
                if (req2 != null)
                {
                    return View(new ViewNotesViewModel { Admin = req.Adminnotes, Physician = req.Physiciannotes, Transfer = req1.Admin.Firstname + "transferred to Dr." + req2.Lastname + "on" + req1.Createddate + "at" + req1.Createddate.TimeOfDay + ":test assign", req_id = id });
                }
            }//return View(model);
            return View(new ViewNotesViewModel { Admin = req.Adminnotes, Physician = req.Physiciannotes, req_id = id });


        }
        [HttpPost]
        public IActionResult ViewNotes(ViewNotesViewModel Vw)
        {
            var admin = HttpContext.Session.GetString("Admin_User");
            Vw.adminName = admin;
            viewNotes.AddOrUpdateViewNote(Vw);
            return RedirectToAction("ViewNotes", new { id = Vw.req_id });
        }
        [HttpPost]
        public IActionResult CancelCase(CancelCaseViewModel Vw)
        {
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == Vw.Id);
            req.Status = 3;
            req.Casetag = Vw.reason;
            req.Modifieddate = DateTime.Now;
            applicationDb.Requests.Update(req);
            var req1 = new Requeststatuslog { Requestid = req.Requestid, Status = 3, Notes = Vw.admin_notes, Createddate = DateTime.Now };
            applicationDb.Requeststatuslogs.Add(req1);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
        }
        [HttpPost]
        public IActionResult BlockCase(BlockCaseViewModel Vw)
        {
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == Vw.Id);
            req.Status = 11;
            req.Modifieddate = DateTime.Now;
            applicationDb.Requests.Update(req);
           
            var req1 = new Requeststatuslog { Requestid = req.Requestid, Status = 11, Notes = Vw.admin_notes, Createddate = DateTime.Now };
            applicationDb.Requeststatuslogs.Add(req1);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
        }
        [HttpPost]
        public IActionResult AssignCase(AssignCaseViewModel Vw)
        {
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == Vw.Id);
            req.Status = 2;
            req.Physicianid = Vw.physician;
            req.Modifieddate = DateTime.Now;
            applicationDb.Requests.Update(req);
            var req1 = new Requeststatuslog { Requestid = req.Requestid, Status = 2, Notes = Vw.admin_notes, Createddate = DateTime.Now };
            applicationDb.Requeststatuslogs.Add(req1);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
        }
        public List<DataViewModel> GetPhysicianByRegionId(int regionId)
        {
           var DataTableViewModels=from physician in applicationDb.Physicians where physician.Regionid== regionId select new DataViewModel { First=physician.Firstname,Last=physician.Lastname,id=physician.Physicianid};
            return DataTableViewModels.ToList();
        }
        public IActionResult Download(int id)
        {
            var path = (applicationDb.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }
        public void uploadFile(List<IFormFile> file, int id)
        {

            foreach (var item in file)
            {
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", item.FileName);

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
        [HttpPost]
        public IActionResult ViewUploads(int id,List<IFormFile> files)
        {

            var r = applicationDb.Requests.FirstOrDefault(m => m.Requestid == id);
            foreach (var item in files)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r.Requestid.ToString() + item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                applicationDb.Add(r2);
                applicationDb.SaveChanges();
                if (files != null)
                {
                    uploadFile(files, r.Requestid);
                }
            }
            return RedirectToAction("ViewUploads", new {id=r.Requestid});
        }
    }
}
