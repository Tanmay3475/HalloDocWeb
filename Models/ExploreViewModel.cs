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
        public enum statusName
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
        public enum requestor
        {
            hello,
            world,
            Friend,
            Concierge,
            Business,
            VIP,
            Patient
        }
        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }
        public string findRequestor(int status)
        {
            string sName = ((requestor)status).ToString();
            return sName;
        }
        public int status { get; set; }
        public int requestorId { get; set; }
        public DateOnly dateofService { get; set; }
        public DateOnly closeCaseDate { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string zip { get; set; }
        public string physician_note { get; set; }
        public string cancelled { get; set; }
        public string admin_note { get; set; }
        public string patient_note { get; set; }
        public int Id { get; set; }
    }
}
