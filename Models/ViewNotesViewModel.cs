using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class ViewNotesViewModel
    {
        public string Physician { get; set; }
        public string Admin { get; set; }
        public string Transfer { get; set; }
        public int req_id { get; set; }
        public string req_data { get; set; }
        public int admin_id {  get; set; } 
        public string PhysicianName {  get; set; } 
        public string adminName {  get; set; } 
        public DateTime createddate {  get; set; } 
    }
}
