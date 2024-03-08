using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HalloDoc.Repository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly ApplicationDbContext _db;
        public RequestRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Request request)
        {
            _db.Requests.Update(request);
        }
        public List<AdminDashboardTableDataViewModel> getAllAdminDashboardTableData(int status)
        {
            var AdminDashboardDataTableViewModels = from user in _db.Users
                                                    join req in _db.Requests on user.Userid equals req.Userid
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
                                                        ProviderEmail = _db.Physicians.FirstOrDefault(x => x.Physicianid == req.Physicianid).Email,
                                                        PatientEmail = user.Email,
                                                        RequestorEmail = req.Email,
                                                        RequestorType = req.Requesttypeid,
                                                        requestid = req.Requestid,
                                                    };
            return AdminDashboardDataTableViewModels.ToList();
        }
    }
}
