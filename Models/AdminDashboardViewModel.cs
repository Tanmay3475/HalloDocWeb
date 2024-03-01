using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class AdminDashboardViewModel
    {
        public List<AdminDashboardTableDataViewModel> adminDashboardTableDataViewModels { get; set; }
        public List<Request> Requests { get; set; }
         
    }
}
