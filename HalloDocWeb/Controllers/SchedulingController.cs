using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
//using static Vonage.VonageUrls;

namespace HelloDoc.Controllers.Scheduling
{
    public class SchedulingController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SchedulingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Scheduling()
        {
            SchedulingViewModel modal = new SchedulingViewModel();
            modal.regions = _context.Regions.ToList();
            modal.physicians=_context.Physicians.ToList();
            return View(modal);
        }
        public IActionResult RequestedShift()
        {
            SchedulingViewModel modal = new SchedulingViewModel();
            modal.regions = _context.Regions.ToList();
            modal.shiftdetails = _context.Shiftdetails.Include(u => u.Shift).AsEnumerable().Where(m => m.Status == 0 && (m.Isdeleted[0] == false)).ToList();
            modal.physicians = _context.Physicians.ToList();
            return View(modal);
        }
        [HttpPost]
        public IActionResult ApproveShift(List<int> check)
        {
            foreach (var item in check)
            {
                var req = _context.Shiftdetails.FirstOrDefault(m => m.Shiftdetailid == item);
                req.Status = 1;
                _context.Shiftdetails.Update(req);
                _context.SaveChanges();
            }

            return RedirectToAction("RequestedShift");
        }
        [HttpPost]
        public IActionResult DeleteShift(List<int> check)
        {
            foreach (var item in check)
            {
                var req = _context.Shiftdetails.FirstOrDefault(m => m.Shiftdetailid == item);
                req.Isdeleted[0] = true;
                _context.Shiftdetails.Update(req);
                _context.SaveChanges();
            }

            return RedirectToAction("RequestedShift");
        }
        public IActionResult ProviderOnCall(int regionid)
        {
            SchedulingViewModel modal = new SchedulingViewModel();
            modal.regions = _context.Regions.ToList();
            if (regionid == 0)
            {
                modal.active = _context.Physicians.Where(m => m.Status == 1).ToList();
                modal.inactive = _context.Physicians.Where(m => m.Status == 2).ToList();
            }
            else
            {
                modal.active = _context.Physicians.Where(m => m.Status == 1 && m.Physicianregions.Any(m => m.Regionid == regionid) == true).ToList();
                modal.inactive = _context.Physicians.Where(m => m.Status == 2 && m.Physicianregions.Any(m => m.Regionid == regionid) == true).ToList();

            }
            modal.selectedregionid = regionid;
            return View(modal);
        }


        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid, int status)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _context.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _context.Physicians.ToList();
            }

            switch (PartialName)
            {

                case "_DayWise":
                    DayWiseScheduling day = new DayWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.Shiftdetails.Include(u => u.Shift).AsEnumerable().Where(m => m.Isdeleted[0] == false && m.Status == status).ToList()
                    };
                    return PartialView("_DayWise", day);

                case "_WeekWise":
                    WeekWiseScheduling week = new WeekWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.Shiftdetails.Include(u => u.Shift).AsEnumerable().Where(m => m.Isdeleted[0] == false && m.Status == status).ToList()

                    };
                    return PartialView("_WeekWise", week);

                case "_MonthWise":
                    MonthWiseScheduling month = new MonthWiseScheduling
                    {
                        date = currentDate,
                        physicians = physician,
                        shiftdetails = _context.Shiftdetails.Include(u => u.Shift).AsEnumerable().Where(m => m.Isdeleted[0] == false && m.Status == status).ToList()
                    };
                    return PartialView("_MonthWise", month);

                default:
                    return PartialView("_DayWise");
            }
        }

        public List<Physician> filterregion(string regionid)
        {
            List<Physician> physicians = _context.Physicianregions.Where(u => u.Regionid.ToString() == regionid).Select(y => y.Physician).ToList();
            return physicians;
        }

        public IActionResult AddShift(SchedulingViewModel model)
        {
            int adminid = (int)HttpContext.Session.GetInt32("Admin_Id");
            var admin = _context.Admins.FirstOrDefault(m => m.Adminid == adminid);
            Aspnetuser aspnetadmin = _context.Aspnetusers.FirstOrDefault(m => m.AspNetUserId == admin.Aspnetuserid);
            var chk = Request.Form["repeatdays"].ToList();
            var shiftid = _context.Shifts.Where(u => u.Physicianid == model.providerid).Select(u => u.Shiftid).ToList();
            if (shiftid.Count() > 0)
            {
                foreach (var obj in shiftid)
                {
                    var shiftdetailchk = _context.Shiftdetails.Where(u => u.Shiftid == obj && u.Shiftdate == model.shiftdate).ToList();
                    if (shiftdetailchk.Count() > 0)
                    {
                        foreach (var item in shiftdetailchk)
                        {
                            if ((model.starttime >= new DateTime(item.Shiftdate.Day, item.Shiftdate.Month, item.Shiftdate.Year, item.Starttime.Hour, item.Starttime.Minute, item.Starttime.Second) && model.starttime <= new DateTime(item.Shiftdate.Day, item.Shiftdate.Month, item.Shiftdate.Year, item.Endtime.Hour, item.Endtime.Minute, item.Endtime.Second) || (model.endtime >= new DateTime(item.Shiftdate.Day, item.Shiftdate.Month, item.Shiftdate.Year, item.Starttime.Hour, item.Starttime.Minute, item.Starttime.Second) && model.endtime <= new DateTime(item.Shiftdate.Day, item.Shiftdate.Month, item.Shiftdate.Year, item.Endtime.Hour, item.Endtime.Minute, item.Endtime.Second))))
                            {
                                TempData["error"] = "Shift is already assigned in this time";
                                return RedirectToAction("Scheduling");
                            }
                        }
                    }
                }
            }
            Shift shift = new Shift
            {
                Physicianid = model.providerid,
                Startdate = DateOnly.FromDateTime(model.shiftdate),
                Repeatupto = model.repeatcount,
                Createddate = DateTime.Now,
                Createdby = aspnetadmin.AspNetUserId
            };
            foreach (var obj in chk)
            {
                shift.Weekdays += obj;
            }
            if (model.repeatcount > 0)
            {
                shift.Isrepeat = new BitArray(new[] { true });
            }
            else
            {
                shift.Isrepeat = new BitArray(new[] { false });
            }
            _context.Shifts.Add(shift);
            _context.SaveChanges();
            DateTime curdate = model.shiftdate;
            Shiftdetail shiftdetail = new Shiftdetail();
            shiftdetail.Shiftid = shift.Shiftid;
            shiftdetail.Shiftdate = curdate;
            shiftdetail.Regionid = model.regionid;
            shiftdetail.Starttime = new TimeOnly(model.starttime.Hour, model.starttime.Minute, model.starttime.Second);
            shiftdetail.Endtime = new TimeOnly(model.endtime.Hour, model.endtime.Minute, model.endtime.Second);
            shiftdetail.Isdeleted = new BitArray(new[] { false });
            _context.Shiftdetails.Add(shiftdetail);
            _context.SaveChanges();

            var dayofweek = model.shiftdate.DayOfWeek.ToString();
            int valueforweek;
            if (dayofweek == "Sunday")
            {
                valueforweek = 0;
            }
            else if (dayofweek == "Monday")
            {
                valueforweek = 1;
            }
            else if (dayofweek == "Tuesday")
            {
                valueforweek = 2;
            }
            else if (dayofweek == "Wednesday")
            {
                valueforweek = 3;
            }
            else if (dayofweek == "Thursday")
            {
                valueforweek = 4;
            }
            else if (dayofweek == "Friday")
            {
                valueforweek = 5;
            }
            else
            {
                valueforweek = 6;
            }
            if (shift.Isrepeat[0] == true)
            {
                for (int j = 0; j < shift.Weekdays.Count(); j++)
                {
                    var z = shift.Weekdays;
                    var p = shift.Weekdays.ElementAt(j).ToString();
                    int ele = Int32.Parse(p);
                    int x;
                    if (valueforweek > ele)
                    {
                        x = 6 - valueforweek + 1 + ele;
                    }
                    else
                    {
                        x = ele - valueforweek;
                    }
                    if (x == 0)
                    {
                        x = 7;
                    }
                    DateTime newcurdate = model.shiftdate.AddDays(x);
                    for (int i = 0; i < model.repeatcount; i++)
                    {
                        Shiftdetail shiftdetailnew = new Shiftdetail
                        {
                            Shiftid = shift.Shiftid,
                            Shiftdate = newcurdate,
                            Regionid = model.regionid,
                            Starttime = new TimeOnly(model.starttime.Hour, model.starttime.Minute, model.starttime.Second),
                            Endtime = new TimeOnly(model.endtime.Hour, model.endtime.Minute, model.endtime.Second),
                            Isdeleted = new BitArray(new[] { false })
                        };
                        _context.Shiftdetails.Add(shiftdetailnew);
                        _context.SaveChanges();
                        newcurdate = newcurdate.AddDays(7);
                    }
                }
            }
            return RedirectToAction("tabs", "AdminStatus");
        }
        //public IActionResult viewShiftEdit(SchedulingViewModel obj)
        //{
        //    var currentDate = DateTime.Parse(date);
        //    List<Physician> physician = _context.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
        //    if (regionid == 0)
        //    {
        //        physician = _context.Physicians.ToList();
        //    }
        //    DayWiseScheduling day = new DayWiseScheduling
        //    {
        //        date = currentDate,
        //        physicians = physician,
        //        shiftdetails = _context.Shiftdetails.Include(u => u.Shift).ToList()
        //    };
        //    return PartialView("_DayWise", day);
        //}
     
        public SchedulingViewModel ViewShift(int id)
        {
            var shift = _context.Shiftdetails.Include(u => u.Shift).FirstOrDefault(m => m.Shiftdetailid == id);
            var x = new SchedulingViewModel { 
                regionname=_context.Regions.FirstOrDefault(r=>r.Regionid==shift.Regionid).Name,
                physicianname=_context.Physicians.FirstOrDefault(r=>r.Physicianid==shift.Shift.Physicianid).Firstname+" "+ _context.Physicians.FirstOrDefault(r => r.Physicianid == shift.Shift.Physicianid).Lastname,
                shifttoday = new DateOnly(shift.Shiftdate.Year, shift.Shiftdate.Month, shift.Shiftdate.Day), 
                starttime = new DateTime(shift.Shiftdate.Year, shift.Shiftdate.Month, shift.Shiftdate.Day, shift.Starttime.Hour, shift.Starttime.Minute, shift.Starttime.Second), 
                endtime = new DateTime(shift.Shiftdate.Year, shift.Shiftdate.Month, shift.Shiftdate.Day, shift.Endtime.Hour, shift.Endtime.Minute, shift.Endtime.Second) 
            };
            return x;
        }
    }
}
