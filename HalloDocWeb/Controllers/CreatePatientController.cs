using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Models.DataContext;
using System.Collections;
using Microsoft.Extensions.Hosting;
using HalloDoc.Models;


namespace HalloDocWeb.Controllers
{
   
    public class CreatePatientController : Controller
    {
         
        private readonly ApplicationDbContext _context;
        

        public CreatePatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create_patient()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create_patient(CreatePatientViewModel s)
        {
            
            Request r= new Request { Requesttypeid=1,Status=1,Firstname=s.FirstName,Lastname=s.LastName,Createddate=DateTime.Now,Isurgentemailsent = new BitArray(1), Confirmationnumber = s.FirstName + DateTime.Now };
            _context.Add(r);
            _context.SaveChanges();
            Requestclient r1 = new Requestclient { Requestid = r.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.PhoneNumber, Address = s.Room, Notes = s.Symptoms, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month), Intdate = s.DateOfBirth.Day };
            _context.Add(r1);
            _context.SaveChanges();
            foreach (var item in s.Filepath)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r1.Requestid.ToString()+ item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                _context.Add(r2);
                _context.SaveChanges();
                if (s.Filepath != null)
                {
                    uploadFile(s.Filepath,r1.Requestid);
                }
            }
            return RedirectToAction("Submit_request", "Home");
        }
        public void uploadFile(List<IFormFile> file, int id)
        {

            foreach (var item in file)
            {
                
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", id.ToString()+item.FileName);
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
        public IActionResult Friend_request()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Friend_request(CreateFriendViewModel s)
        {
            Request r = new Request { Requesttypeid = 2, Status = 1, Firstname = s.firstName, Lastname = s.lastName, Createddate = DateTime.Now,Relationname=s.Relationname,Email=s.email,Phonenumber=s.phonenumber,Isurgentemailsent=new BitArray(1), Confirmationnumber = s.firstName + DateTime.Now };
            _context.Add(r);
            _context.SaveChanges();
            Requestclient r1 = new Requestclient { Requestid = r.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.PhoneNumber, Address = s.Room, Notes = s.Symptoms, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month), Intdate = s.DateOfBirth.Day };
            _context.Add(r1);
            _context.SaveChanges();
            foreach (var item in s.Filepath)
            {
                string fname = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Documents", r.Requestid.ToString(),item.FileName);
                Requestwisefile r2 = new Requestwisefile { Requestid = r.Requestid, Filename = fname, Createddate = DateTime.Now };
                _context.Add(r2);
                _context.SaveChanges();
                if (s.Filepath != null)
                {
                    uploadFile(s.Filepath, r1.Requestid);
                }
            }
            return RedirectToAction("Submit_request","Home");
        }
        public IActionResult Concierge_request()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Concierge_request(ConciergeViewModel s)
        {
            Concierge r = new Concierge { Conciergename = s.Conciergename, Street =s.Street,City=s.City,Zipcode=s.ZipCode,State=s.State,Createddate=DateTime.Now,Regionid=1};
            _context.Add(r);
            _context.SaveChanges();
            Request r1 = new Request { Requesttypeid = 3, Status = 1, Firstname = s.firstName, Lastname = s.lastName, Createddate = DateTime.Now,  Email = s.email, Phonenumber = s.phoneNumber, Isurgentemailsent = new BitArray(1) ,Confirmationnumber = s.firstName +DateTime.Now };
            _context.Add(r1);
            _context.SaveChanges();
            Requestclient r2 = new Requestclient { Requestid = r1.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.PhoneNumber, Address = s.Room, Notes = s.Symptoms, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month) ,Intdate=s.DateOfBirth.Day};
            _context.Add(r2);
            _context.SaveChanges();
            Requestconcierge r3=new Requestconcierge { Conciergeid=r.Conciergeid,Requestid=r1.Requestid};
           _context.Add(r3);
            _context.SaveChanges();
            return RedirectToAction("Submit_request", "Home");
        }
        public IActionResult Business_request()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Business_request(BusinessViewModel s)
        {
            
            Business r = new Business { Name = s.Name,  Createddate = DateTime.Now };
            _context.Add(r);
            _context.SaveChanges();
            Request r1 = new Request { Requesttypeid = 4, Status = 1, Firstname = s.firstName, Lastname = s.lastName, Createddate = DateTime.Now, Email = s.email, Phonenumber = s.phoneNumber , Isurgentemailsent = new BitArray(1) ,Confirmationnumber=s.firstName+DateTime.Now};
            _context.Add(r1);
            _context.SaveChanges();
            Requestclient r2 = new Requestclient { Requestid = r1.Requestid, Firstname = s.FirstName, Lastname = s.LastName, Phonenumber = s.PhoneNumber, Address = s.Room, Notes = s.Symptoms, Email = s.Email, City = s.City, Zipcode = s.ZipCode, State = s.State, Intyear = s.DateOfBirth.Year, Strmonth = Convert.ToString(s.DateOfBirth.Month), Intdate = s.DateOfBirth.Day };
            _context.Add(r2);
            
            _context.SaveChanges();
            Requestbusiness r3 = new Requestbusiness {Businessid=r.Businessid, Requestid = r1.Requestid };
            _context.Add(r3);
            _context.SaveChanges();
           
            return RedirectToAction("Submit_request", "Home");
        }
    }
}
