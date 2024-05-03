using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

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
            
            return View(new ExploreViewModel());
        }
        public IActionResult Explore(int Id)
        {
            var req = _context.Requests.AsEnumerable().Where(m => m.Userid == Id).ToList();
            var req1 = new List<ExploreViewModel>();
            foreach (var re in req)
            {
                var r = new ExploreViewModel { Name = re.Firstname, CreatedDate = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day), Confirmation = re.Confirmationnumber, status = re.Status };
                var res = _context.Requeststatuslogs.AsEnumerable().FirstOrDefault(m => m.Status == 6 && m.Requestid == re.Requestid);
                if (res != null)
                {
                    r.ConcludedDate = new DateOnly(res.Createddate.Year, res.Createddate.Month, res.Createddate.Day);
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
        public IActionResult Delete(int Id)
        {
            var req = _context.Requests.AsEnumerable().Where(m => m.Userid == Id).ToList();
            var req1 = new List<ExploreViewModel>();
            foreach (var re in req)
            {
                var r = new ExploreViewModel { Name = re.Firstname, CreatedDate = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day), Confirmation = re.Confirmationnumber, status = re.Status };
                var res = _context.Requeststatuslogs.AsEnumerable().FirstOrDefault(m => m.Status == 6 && m.Requestid == re.Requestid);
                if (res != null)
                {
                    r.ConcludedDate = new DateOnly(res.Createddate.Year, res.Createddate.Month, res.Createddate.Day);
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
            if (model.FirstName == null && model.LastName == null && model.Email == null && model.Phone == null)
            {
                var req = _context.Users.ToList();
                var req1 = new List<RecordViewModel>();
                foreach (var record in req)
                {
                    var req2 = new RecordViewModel { Id = record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
                    req1.Add(req2);
                }
                return View(req1);
            }
            else
            {
                var req = _context.Users.Where(m => m.Email.ToLower().Contains(model.Email.ToLower()) && m.Firstname.ToLower().Contains(model.FirstName.ToLower()) && m.Lastname.ToLower().Contains(model.LastName.ToLower()) && m.Mobile.Contains(model.Phone)).ToList();
                var req1 = new List<RecordViewModel>();
                foreach (var record in req)
                {
                    var req2 = new RecordViewModel { Id = record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
                    req1.Add(req2);
                }
                return View(req1);

            }
        }
        public IActionResult SearchPartial()
        {
            //if (model.FirstName == null && model.LastName == null && model.Email == null && model.Phone == null)
            //{
            //    var req = _context.Users.ToList();
            //    var req1 = new List<RecordViewModel>();
            //    foreach (var record in req)
            //    {
            //        var req2 = new RecordViewModel { Id = record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
            //        req1.Add(req2);
            //    }
            //    return View(req1);
            //}
            //else
            //{
            //    var req = _context.Users.Where(m => m.Email.ToLower().Contains(model.Email.ToLower()) && m.Firstname.ToLower().Contains(model.FirstName.ToLower()) && m.Lastname.ToLower().Contains(model.LastName.ToLower()) && m.Mobile.Contains(model.Phone)).ToList();
            //    var req1 = new List<RecordViewModel>();
            //    foreach (var record in req)
            //    {
            //        var req2 = new RecordViewModel { Id = record.Userid, FirstName = record.Firstname, LastName = record.Lastname, Email = record.Email, Phone = record.Mobile, Address = record.Street };
            //        req1.Add(req2);
            //    }
            //return View(req1);}
            var req = _context.Requests.Include(re => re.Requestclients).Include(re => re.Physician).Include(re => re.Requeststatuslogs).Include(re => re.Requestnotes).ToList();
            var req1 = new List<ExploreViewModel>();
            foreach (var re in req)
            {
                var req2 = _context.Requestclients.FirstOrDefault(m => m.Requestid == re.Requestid);
                var req3 = _context.Requestnotes.FirstOrDefault(m => m.Requestid == re.Requestid);
                var req4 = _context.Physicians.FirstOrDefault(m => m.Physicianid == re.Physicianid);
                var req5 = _context.Requeststatuslogs.FirstOrDefault(m => m.Requestid == re.Requestid && m.Status == 3);
                if (req2 != null)
                {
                    if (req3 != null)
                    {
                        if (req5 != null)
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    admin_note = req3.Adminnotes,
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    Pro = req4.Firstname,
                                    physician_note = req3.Physiciannotes,
                                    patient_note = req2.Notes
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    admin_note = req3.Adminnotes,
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    patient_note = req2.Notes
                                };
                                req1.Add(r);
                            }
                        }
                        else
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    admin_note = req3.Adminnotes,
                                    Pro = req4.Firstname,

                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    admin_note = req3.Adminnotes,
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }

                    }
                    else
                    {
                        if (req5 != null)
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,

                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    Pro = req4.Firstname,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }
                        else
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    Pro = req4.Firstname,

                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    address = req2.Address,
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    patient_note = req2.Notes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }
                    }
                }
                else
                {
                    if (req3 != null)
                    {
                        if (req5 != null)
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    admin_note = req3.Adminnotes,
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    Pro = req4.Firstname,
                                    physician_note = req3.Physiciannotes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    admin_note = req3.Adminnotes,
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }
                        else
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    admin_note = req3.Adminnotes,
                                    Pro = req4.Firstname,

                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    zip = req2.Zipcode,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    admin_note = req3.Adminnotes,
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    physician_note = req3.Physiciannotes,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }

                    }
                    else
                    {
                        if (req5 != null)
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {

                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    Pro = req4.Firstname,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    cancelled = req5.Notes,
                                    closeCaseDate = new DateOnly(req5.Createddate.Year, req5.Createddate.Month, req5.Createddate.Day),
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    Id = re.Requestid
                                };
                                req1.Add(r);
                            }
                        }
                        else
                        {
                            if (req4 != null)
                            {
                                var r = new ExploreViewModel
                                {
                                    Pro = req4.Firstname,

                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    Id = re.Requestid
                                };
                                req1.Add(r);

                            }
                            else
                            {
                                var r = new ExploreViewModel
                                {
                                    dateofService = new DateOnly(re.Createddate.Year, re.Createddate.Month, re.Createddate.Day),
                                    email = re.Email,
                                    Name = re.Firstname,
                                    requestorId = re.Requesttypeid,
                                    phone = re.Phonenumber,
                                    status = re.Status,
                                    Id=re.Requestid
                                };
                                req1.Add(r);
                            }
                        }
                    }
                }
            }
                return View(req1);


            
        }
    }
}
