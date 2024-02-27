﻿using HalloDoc.Models;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace HalloDocWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _db;
        private readonly IAspnetuserRepository _db1;
        public AdminController(IAdminRepository db,IAspnetuserRepository db1)
        {
            _db=db;
            _db1=db1;
        }
        public IActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Admin_Login(AdminLoginViewModel a)
        {
            if(ModelState.IsValid)
            {
                var aspnetuser = _db1.GetFirstOrDefault(m=>m.Username==a.Username&& m.Passwordhash==a.Password);
                if(aspnetuser!=null)
                {
                    var admin = _db.GetFirstOrDefault(m => m.Aspnetuserid == aspnetuser.AspNetUserId);
                    int id = admin.Adminid;
                    HttpContext.Session.SetInt32("Admin_Id", id);
                    return RedirectToAction("Admin_Dashboard");
                }
            }
            return NotFound();
        }
        public IActionResult Admin_Reset()
        {
            return View();
        }
        public IActionResult Admin_Dashboard()
        {
            return View();
        }
    }
}