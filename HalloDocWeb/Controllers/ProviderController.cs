using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HalloDocWeb.Controllers
{
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext context;
        public ProviderController(ApplicationDbContext application)
        {
            context = application;
        }
        public IActionResult Information()
        {
            var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped, id = pro.Physicianid, regions = context.Regions.ToList(), regid = (int)pro.Regionid }).ToList();
            return View(req);
        }
        public IActionResult Edit(int Id)
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var req = context.Physicians.FirstOrDefault(context => context.Physicianid == Id);
            var req1 = context.Aspnetusers.FirstOrDefault(c => c.AspNetUserId == req.Aspnetuserid);
            var reglist = context.Physicianregions.AsEnumerable().Where(context => context.Physicianid == Id).ToList();
            var pro = new ProviderViewModel()
            {
                UserName = req1.Username,
                Password = req1.Passwordhash,
                status = (int)req.Status,
                role_id = (int)req.Roleid,
                first_name = req.Firstname,
                last_name = req.Lastname,
                email = req.Email,
                phone = req.Mobile,
                medicalLicense = req.Medicallicense,
                NPI_number = req.Npinumber,
                syncEmail = req.Syncemailaddress,
                regions = context.Regions.ToList(),
                phyReg = reglist,
                Address1 = req.Address1,
                Address2 = req.Address2,
                City = req.City,
                regid = (int)req.Regionid,
                Pin = req.Zip,
                AltPhone = req.Altphone,
                BusinessName = req.Businessname,
                BusinessWebsite = req.Businesswebsite,
                ind = req.Isagreementdoc,
                hip = req.Isbackgrounddoc,
                back = req.Isbackgrounddoc,
                non_dis = req.Isnondisclosuredoc,
                lic = req.Islicensedoc,
                AdminNotes = req.Adminnotes,
                Id=req.Physicianid,
                roles=context.Roles.AsEnumerable().Where(m=>m.Accounttype==2).ToList()

            };
            return View(pro);
        }
        [HttpPost]
        public IActionResult Edit(string pass,int ID)
        {
            var admin = context.Physicians.FirstOrDefault(a => a.Physicianid == ID);
            var aspnet = context.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            aspnet.Passwordhash = pass;
            context.Aspnetusers.Update(aspnet);
            context.SaveChanges();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("Edit", new {Id=ID});
        }[HttpPost]
        public IActionResult EditAsp(ProviderViewModel model)
        {
            var admin = context.Physicians.FirstOrDefault(a => a.Physicianid == model.Id);
            admin.Roleid = model.role_id;
            admin.Status= (short?)model.status;
            context.Physicians.Update(admin);
            context.SaveChanges();
            var aspnet = context.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            aspnet.Username = model.UserName;
            context.Aspnetusers.Update(aspnet);
            context.SaveChanges();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("Edit", new {Id=model.Id});
        }
        [HttpPost]
        public IActionResult EditProfile(ProviderViewModel model)
        {
            var admin = context.Physicians.FirstOrDefault(a => a.Physicianid == model.Id);
            //var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            //aspnet.Passwordhash = pass;
            //applicationDb.Aspnetusers.Update(aspnet);
            admin.Firstname = model.first_name; admin.Lastname = model.last_name; admin.Email = model.email; admin.Mobile = model.phone;admin.Medicallicense = model.medicalLicense;admin.Npinumber = model.NPI_number;admin.Syncemailaddress = model.syncEmail;
            admin.Modifieddate = DateTime.Now;
            context.Physicians.Update(admin);
            context.SaveChanges();
            List<Region> sel = new List<Region>();
            if (model.selected != null)
            {
                foreach (var item in model.selected)
                {
                    var reg = context.Regions.FirstOrDefault(m => m.Regionid == item);
                    sel.Add(reg);
                }
            }
            List<Region> notsel = new List<Region>();
            if (model.notselected != null)
            {
                foreach (var item in model.notselected)
                {
                    var reg = context.Regions.FirstOrDefault(m => m.Regionid == item);
                    notsel.Add(reg);
                }
            }
            var reg1 = context.Regions.AsEnumerable().Except(sel).ToList();
            var reg2 = context.Regions.AsEnumerable().Except(notsel).ToList();
            foreach (var item in reg1)
            {
                var reg = context.Physicianregions.FirstOrDefault(M => M.Regionid == item.Regionid);
                if (reg != null)
                {
                    context.Physicianregions.Remove(reg);
                    context.SaveChanges();
                }
            }
            foreach (var item in reg2)
            {
                var reg3 = context.Physicianregions.FirstOrDefault(m => m.Regionid == item.Regionid && m.Physicianid == admin.Physicianid);
                if (reg3 == null)
                {
                    var reg = new Physicianregion { Regionid = item.Regionid, Physicianid = admin.Physicianid };
                    context.Physicianregions.Add(reg);
                    context.SaveChanges();
                }
            }
            //var reg2 = applicationDb.Regions.AsEnumerable().Except(notsel).ToList();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("Edit", new {Id=model.Id});
            }
        [HttpPost]
        public IActionResult EditShipProfile(ProviderViewModel model)
        {
            var admin =context.Physicians.FirstOrDefault(a => a.Physicianid == model.Id);
            //var aspnet = applicationDb.Aspnetusers.FirstOrDefault(a => a.AspNetUserId == admin.Aspnetuserid);
            //aspnet.Passwordhash = pass;
            //applicationDb.Aspnetusers.Update(aspnet);
            admin.Address1 = model.Address1; admin.Address2 = model.Address2; admin.City = model.City; admin.Altphone = model.AltPhone; admin.Regionid = model.regid;
            admin.Zip = model.Pin;
            admin.Modifieddate = DateTime.Now;
            context.Physicians.Update(admin);
            context.SaveChanges();
            //var region = from reg in applicationDb.Adminregions where reg.Adminid == id select reg.Regio
            return RedirectToAction("Edit", new { Id = model.Id });

        }
        public IActionResult Create()
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var reg = context.Regions.ToList();
            var role = context.Roles.ToList();
            return View(new ProviderViewModel { regions = reg, roles = role });
        }
        [HttpPost]
        public IActionResult Create(ProviderViewModel model)
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var aspuser = new Aspnetuser { AspNetUserId = DateTime.Now.ToString(), Passwordhash = model.Password, Username = model.UserName, Createddate = DateTime.Now };
            context.Aspnetusers.Add(aspuser);
            context.SaveChanges();
            var bit = new BitArray(1);
            bit[0] = true;
            var physician = new Physician { Status = 1, City = model.City, Address1 = model.Address1, Address2 = model.Address2, Adminnotes = model.AdminNotes, Createdby = aspuser.AspNetUserId, Npinumber = model.NPI_number, Businessname = model.BusinessName, Businesswebsite = model.BusinessWebsite, Aspnetuserid = aspuser.AspNetUserId, Altphone = model.AltPhone, Medicallicense = model.medicalLicense, Email = model.email, Firstname = model.first_name, Lastname = model.last_name, Photo = model.Photo[0].FileName, Isagreementdoc = bit, Isbackgrounddoc = bit, Islicensedoc = bit, Isnondisclosuredoc = bit, Iscredentialdoc = bit, Roleid = model.role_id, Createddate = DateTime.Now, Zip = model.Pin, Mobile = model.phone };
            context.Physicians.Add(physician);
            context.SaveChanges();
            var phyno = new Physiciannotification { Physicianid = physician.Physicianid, Isnotificationstopped = bit };
            context.Physiciannotifications.Add(phyno);
            context.SaveChanges();
            foreach (var item in model.region_id)
            {
                var phy = new Physicianregion { Physicianid = physician.Physicianid, Regionid = item };
                context.Physicianregions.Add(phy);
                context.SaveChanges();
            }
            uploadFile(model.Independent[0], physician.Physicianid, "Independent");
            uploadFile(model.background[0], physician.Physicianid, "Background");
            uploadFile(model.License[0], physician.Physicianid, "License");
            uploadFile(model.hipaa[0], physician.Physicianid, "HIPAA");
            uploadFile(model.Photo[0], physician.Physicianid, "Photo");
            uploadFile(model.Non_disclosure[0], physician.Physicianid, "Non_Disclosure");
            return RedirectToAction("Information");
        }
        [HttpPost]
        public IActionResult CheckNotification(int Id, int check)
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var bit = new BitArray(1);
            bit[0] = true;
            var phyno = context.Physiciannotifications.FirstOrDefault(m => m.Physicianid == Id);
            if (check == 0)
            {
                phyno.Isnotificationstopped[0] = false;
                context.Physiciannotifications.Update(phyno);
                context.SaveChanges();
            }
            else
            {
                phyno.Isnotificationstopped[0] = true;
                context.Physiciannotifications.Update(phyno);
                context.SaveChanges();
            }
            return RedirectToAction("Information");
        }
        public void uploadFile(IFormFile file, int id, string upload)
        {


            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", item.FileName);
            string extension = Path.GetExtension(file.FileName);
            string filename = upload + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Onboarding", id.ToString(), filename);
            string folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Onboarding", id.ToString());

            if (!Directory.Exists(folderpath))
                Directory.CreateDirectory(folderpath);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
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
}

