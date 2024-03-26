using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class AdminDashboardTableDataViewModel
    {
        public List<Request> Requests { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDOB { get; set;}

        public string RequestorName { get; set;}
        public DateTime RequestedDate {  get; set;}
        public string PatientPhone {  get; set;}
        public string PatientEmail { get; set;}
        public string Address { get; set;}
        public string Notes { get; set;}
        public string ProviderEmail { get; set;}
        public string RequestorEmail { get; set;}
        public int RequestorType { get; set;}
        public int requestid { get;set;}
        public string RequestorPhone {  get; set;}
        public List<Casetag> casetags { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Region> regions { get; set; }
        public int? regionid { get; set;}
        
    }
}
