using HalloDoc.Models;
using HalloDoc.Models.DataContext;
using Microsoft.AspNetCore.Mvc;

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
    }
}
