using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HalloDocWeb.Controllers
{
    
    public class AdminStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _db;
        private readonly IRequestRepository _db1;
        private readonly IRequestClientRepository _db2;
        private readonly IPhysicianRepository _db3;
        public AdminStatusController(IUserRepository db, IRequestRepository db1,IRequestClientRepository db2, IPhysicianRepository db3,ApplicationDbContext context) { 
        _db=db;
        _db1=db1;
            _db2 = db2;
            _db3= db3;
            _context=context;
        }
        public IActionResult tabs()
        {
            AdminDashboardViewModel A = new AdminDashboardViewModel
            {
                Requests = (from user in _context.Users join req in _context.Requests on user.Userid equals req.Userid select req).ToList(),
                adminDashboardTableDataViewModels = getallAdminDashboard(1)
            };
            
            return View(A);
        }
      
        public IActionResult New()
        {
            var adminlist = getallAdminDashboard(1);
            return View(adminlist);
        }
        public IActionResult Pending()
        {
            var adminlist = getallAdminDashboard(2);
            return View(adminlist);
        }
        public IActionResult Active()
        {
            var adminlist = getallAdminDashboard(4);
            adminlist.AddRange(getallAdminDashboard(5));
            return View(adminlist);
        }
        public IActionResult Conclude()
        {
            var adminlist = getallAdminDashboard(6);
            return View(adminlist);
        }
        public IActionResult Close()
        {
            var adminlist = getallAdminDashboard(3);
            adminlist.AddRange(getallAdminDashboard(7));
            adminlist.AddRange(getallAdminDashboard(8));
            return View(adminlist);
        }
        public IActionResult Unpaid()
        {
            var adminlist = getallAdminDashboard(9);
            return View(adminlist);
        }



        public List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status)
        {
            var casetag = _context.Casetags.ToList();
            var region=_context.Regions.ToList();
            var provider=_context.Physicians.ToList();
            var AdminDashboardDataTableViewModels = from user in _context.Users
            join req in _context.Requests on user.Userid equals req.Userid
                                                    where req.Status == status
                                                    orderby req.Createddate descending
                                                    select new AdminDashboardTableDataViewModel
                                                    {
                                                        PatientName = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Firstname + " " + req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Lastname,
                                                        PatientDOB = new DateTime(Convert.ToInt32(user.Intyear), Convert.ToInt32(user.Strmonth), Convert.ToInt32(user.Intdate)),
                                                        RequestorName = req.Firstname + " " + req.Lastname,
                                                        RequestedDate = req.Createddate,
                                                        PatientPhone = user.Mobile,
                                                        RequestorPhone = req.Phonenumber,
                                                        Address = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Address,
                                                        Notes = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Notes,
                                                        ProviderEmail = _context.Physicians.FirstOrDefault(x => x.Physicianid ==req.Physicianid).Email,
                                                        PatientEmail = user.Email,
                                                        RequestorEmail = req.Email,
                                                        RequestorType = req.Requesttypeid,
                                                        requestid = req.Requestid,
                                                        casetags=casetag,
                                                        regions=region,
                                                        physicians=provider
                                                    };
            return AdminDashboardDataTableViewModels.ToList();

        }
    }
}
