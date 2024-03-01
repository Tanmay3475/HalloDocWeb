using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
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
        public IActionResult Admin_Dashboard_New(int status)
        {
            AdminDashboardViewModel A = new AdminDashboardViewModel
            {
                Requests = _context.Requests.ToList(),
                adminDashboardTableDataViewModels = getallAdminDashboard(1)
            };
            
            return View(A);
        }
        
        public List<AdminDashboardTableDataViewModel> getallAdminDashboard(int status)
        {
            var AdminDashboardDataTableViewModels = from user in _context.Users
            join req in _context.Requests on user.Userid equals req.Userid
                                                    join reqclient in _context.Requestclients on req.Requestid equals reqclient.Requestid
                                                    where req.Status == status
                                                    orderby req.Createddate descending
                                                    select new AdminDashboardTableDataViewModel
                                                    {
                                                        PatientName = reqclient.Firstname + " " + reqclient.Lastname,
                                                        PatientDOB = new DateTime(Convert.ToInt32(user.Intyear), Convert.ToInt32(user.Strmonth), Convert.ToInt32(user.Intdate)),
                                                        RequestorName = req.Firstname + " " + req.Lastname,
                                                        RequestedDate = req.Createddate,
                                                        PatientPhone = user.Mobile,
                                                        RequestorPhone = req.Phonenumber,
                                                        Address = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Address,
                                                        Notes = req.Requestclients.FirstOrDefault(x => x.Requestid == req.Requestid).Notes,
                                                        ProviderEmail = _db3.GetFirstOrDefault(x => x.Physicianid == req.Physicianid).Email,
                                                        PatientEmail = user.Email,
                                                        RequestorEmail = req.Email,
                                                        RequestorType = req.Requesttypeid,
                                                        requestid = req.Requestid,
                                                    };
            return AdminDashboardDataTableViewModels.ToList();

        }
    }
}
