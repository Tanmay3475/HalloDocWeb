using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class MyProfileViewModel
    {
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string state { get; set; }
        public int regid { get; set; }
        public string password { get; set; }
        public List<Region> regions { get; set; }
        public List<Region> All { get; set; }
        public List<Region> regionexcept { get; set; }
        public List<Adminregion> adminregionlist { get; set; }
        public List<int> selected {  get; set; }
        public List<int> notselected { get; set; }
        public string altphone { get; set; }
        
    }
}
