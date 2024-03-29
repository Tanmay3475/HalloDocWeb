using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;
using Org.BouncyCastle.Ocsp;
using DocumentFormat.OpenXml.Math;

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
            var region = applicationDb.Regions.ToList();
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == id);
            var req1 = applicationDb.Requestclients.FirstOrDefault(req => req.Requestid == id);
            var reg = applicationDb.Regions.FirstOrDefault(r => r.Regionid == req1.Regionid);
            return View(new ViewCaseViewModel {regions=region, id=id, Address = req1.Street, PhoneNumber = req.Phonenumber, Confirmation = req.Confirmationnumber, DateOfBirth = new DateTime(Convert.ToInt32(req1.Intyear), Convert.ToInt32(req1.Strmonth), Convert.ToInt32(req1.Intdate)), Region = reg.Name, Email = req.Email, LastName = req.Lastname, FirstName = req.Firstname, Room = req1.Address, Symptoms = req1.Notes });
        } 
        public IActionResult CloseCase(int id)
        {
            
            var request = applicationDb.Requests.FirstOrDefault(m => m.Requestid == id);
            var user = applicationDb.Users.FirstOrDefault(m => m.Userid == request.Userid);
            var files = (from m in applicationDb.Requestwisefiles
                         where m.Requestid == id && m.Isdeleted == null
            select m).ToList();
            DashboardViewModel ds = new DashboardViewModel { FirstName = request.Firstname, requestwisefiles = files, requestid = id, Confirmation = request.Confirmationnumber,LastName=request.Lastname, DateOfBirth = new DateTime(Convert.ToInt32(user.Intyear), Convert.ToInt32(user.Strmonth), Convert.ToInt32(user.Intdate)),PhoneNumber=user.Mobile,Email= user.Email };
            return View(ds);
        }
        [HttpPost]
        public IActionResult closeCase(int id)
        {

            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == id);
            req.Status = 9;
            req.Modifieddate = DateTime.Now;
            applicationDb.Requests.Update(req);

            var req1 = new Requeststatuslog { Requestid = req.Requestid, Status = 9, Createddate = DateTime.Now };
            applicationDb.Requeststatuslogs.Add(req1);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
        }
        public IActionResult Orders(int id)
        {
            var req=applicationDb.Healthprofessionaltypes.ToList();
            OrdersViewModel
            viewModel = new OrdersViewModel { healthprofessionaltypes=req};
            return View
                (viewModel);
        }
        [HttpPost]
        public IActionResult Orders(OrdersViewModel ordersViewModel)
        {
            var vendor=applicationDb.Healthprofessionals.FirstOrDefault(m=>(m.Profession==ordersViewModel.profession )&& (m.Vendorname==ordersViewModel.business));
            var req = new Orderdetail { Businesscontact = ordersViewModel.business_contact, Email = ordersViewModel.business_email, Faxnumber = ordersViewModel.fax_number, Noofrefill = ordersViewModel.no_of_refills,Vendorid=vendor.Vendorid,Prescription=ordersViewModel.prescription };
         applicationDb.Orderdetails.Add(req);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs","AdminStatus");
        }
        public IActionResult ViewUploads(int id)
        {
            var request = applicationDb.Requests.FirstOrDefault(m => m.Requestid == id);
            var files = (from m in applicationDb.Requestwisefiles
                         where m.Requestid == id && m.Isdeleted== null
                         select m).ToList();
            DashboardViewModel ds = new DashboardViewModel {FirstName=request.Firstname, requestwisefiles = files, requestid = id, Confirmation = request.Confirmationnumber };
            return View(ds);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var request = applicationDb.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == id);
            var t= new System.Collections.BitArray(1);
            t[0] = true;
            request.Isdeleted = t;
            //var files = (from m in applicationDb.Requestwisefiles
            //             where m.Requestid == id
            //             select m).ToList();
            applicationDb.Requestwisefiles.Update(request);
            applicationDb.SaveChanges();
            return RedirectToAction("ViewUploads",new { id=request.Requestid });
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
            if (req != null)
            {
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
            return View(new ViewNotesViewModel {  req_id = id });


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
                        var file = await applicationDb.Requestwisefiles.FirstOrDefaultAsync(x => x.Requestwisefileid == s);
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
        public IActionResult ClearCase(int id)
        {
            var req = applicationDb.Requests.FirstOrDefault(req => req.Requestid == id);
            req.Status = 11;
            req.Modifieddate = DateTime.Now;
            applicationDb.Requests.Update(req);
           
            var req1 = new Requeststatuslog { Requestid = req.Requestid, Status = 10, Createddate = DateTime.Now };
            applicationDb.Requeststatuslogs.Add(req1);
            applicationDb.SaveChanges();
            return RedirectToAction("tabs", "AdminStatus");
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
        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.tanmaymehta@outlook.com";
            var password = "Tan@3475";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
        [HttpPost]
        public IActionResult SendEmail(String mail)
        {
           
            string subject = "Review Agreement";
            string msg = "Hello World";
            SendEmail(mail, subject, msg);
            return RedirectToAction("tabs", "AdminStatus");
        } 
        [HttpPost]
        public IActionResult SendEmail1(String mail,int Id)
        {
            var req = applicationDb.Physicians.FirstOrDefault(m => m.Physicianid == Id);
            string subject = "Provider Mail";
            SendEmail(req.Email, subject, mail);
            return RedirectToAction("Information", "Provider");
        }
        public List<DataViewModel> GetPhysicianByRegionId(int regionId)
        {
           var DataTableViewModels=from physician in applicationDb.Physicians where physician.Regionid== regionId select new DataViewModel { First=physician.Firstname,Last=physician.Lastname,id=physician.Physicianid};
            return DataTableViewModels.ToList();
        }
        //public IActionResult Download(int id)
        //{
        //    var path = (applicationDb.Requestwisefiles.FirstOrDefault(x => x.Requestwisefileid == id)).Filename;
        //    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "document", filename);
        //    var provider = new FileExtensionContentTypeProvider();
        //    if (!provider.TryGetContentType(path, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }
        //    var bytes = System.IO.File.ReadAllBytes(path);
        //    return File(bytes, contentType, Path.GetFileName(path));
        //}
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
        public IActionResult DeleteAll(List<int> check,int id)
        {
            foreach(var item in check)
            {
                var request = applicationDb.Requestwisefiles.FirstOrDefault(m => m.Requestwisefileid == item);
                var t = new System.Collections.BitArray(1);
                t[0] = true;
                request.Isdeleted = t;
                //var files = (from m in applicationDb.Requestwisefiles
                //             where m.Requestid == id
                //             select m).ToList();
                applicationDb.Requestwisefiles.Update(request);
                applicationDb.SaveChanges();
              
            }
            return RedirectToAction("ViewUploads", new { id = id });
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
