using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class ExploreViewModel
    {
        public string Name { get; set; }
        public DateOnly CreatedDate { get; set; }
        public string Confirmation { get; set; }
        public string Pro { get; set; }
        public DateOnly ConcludedDate { get; set; }
        enum statusName
        {
            first,
            Unassigned,         //New 
            Accepted,           //Pending
            Cancelled,          //To-close
            MDEnRoute,          //Active
            MDOnSite,           //Active
            Conclude,           //Conclude
            CancelledByPatient, //To-close
            Closed,             //To-close
            Unpaid,             //Unpaid
            Clear,
            Block
        }
        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }
        public int status {  get; set; }
    }
}
