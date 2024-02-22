using HalloDocWeb.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocWeb.ViewModels
{
    public class DashboardViewModel
    {

        public User User { get; set; } = null!;
        public List<Request> Requests { get; set; } 
        
       
        public List<Requestwisefile>? requestwisefiles { get; set; }
       
        enum statusName
        {
            january,
            Unassigned,
            Cancelled,
            MdEnRoute,
            MdOnSite,
            Closed,
            Clear,
            Unpaid
        }

        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }

    }
}
