using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
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
            var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            return View(req);
        }
public IActionResult Edit(int Id)
        {
        //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            return View();
        }
        public IActionResult Create()
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var reg = context.Regions.ToList();
            var role=context.Roles.ToList();
            return View(new ProviderViewModel { regions=reg,roles=role});
        }
        [HttpPost]
  public IActionResult Create(ProviderViewModel model)
        {
            //    var req = (from pro in context.Physicians join not in context.Physiciannotifications on pro.Physicianid equals not.Physicianid select new ProviderInfoViewModel { ProviderName = pro.Firstname + ',' + pro.Lastname, Status = (int)pro.Status, BitArray = not.Isnotificationstopped ,id=pro.Physicianid}).ToList();
            var aspuser = new Aspnetuser { AspNetUserId=DateTime.Now.ToString(),Passwordhash=model.Password,Username=model.UserName,Createddate=DateTime.Now};
            context.Aspnetusers.Add(aspuser);
            context.SaveChanges();
            var bit = new BitArray(1);
            bit[0] = true;
            var physician=new Physician {Status=1 ,City=model.City,Address1=model.Address1,Address2=model.Address2, Adminnotes = model.AdminNotes,Createdby=aspuser.AspNetUserId, Npinumber = model.NPI_number, Businessname = model.BusinessName, Businesswebsite = model.BusinessWebsite, Aspnetuserid = aspuser.AspNetUserId, Altphone = model.AltPhone, Medicallicense = model.medicalLicense, Email = model.email, Firstname = model.first_name, Lastname = model.last_name, Photo = model.Photo[0].FileName,Isagreementdoc=bit,Isbackgrounddoc=bit,Islicensedoc=bit,Isnondisclosuredoc=bit,Iscredentialdoc=bit,Roleid=model.role_id,Createddate=DateTime.Now,Zip=model.Pin,Mobile=model.phone};
            context.Physicians.Add(physician);
            context.SaveChanges();
            var phyno = new Physiciannotification { Physicianid=physician.Physicianid,Isnotificationstopped=bit};
            context.Physiciannotifications.Add(phyno);  
            context.SaveChanges();
            foreach(var item in model.region_id)
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
        public void uploadFile(IFormFile file, int id,string upload)
        {


            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadDocument", item.FileName);
            string extension = Path.GetExtension(file.FileName);
            string filename = upload + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot", "Onboarding", id.ToString(),filename);
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

