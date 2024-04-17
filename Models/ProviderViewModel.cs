using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class ProviderViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Role> roles{ get; set; }
        public int role_id { get; set; }
        public string first_name { get; set;}
        public string last_name { get; set;}
        public string email { get; set;}
        public string phone{ get; set; }
        public string medicalLicense { get; set;}
        public string NPI_number { get; set;}
        public List<Region> regions { get; set; }
        public List<int> region_id { get; set;}
        public string Address1 { get; set;} 
        public string Address2 { get; set;}
        public string City { get; set;}
        public string Country { get; set;}
        public string Pin { get; set;}
        public string AltPhone { get; set;}
        public string BusinessName { get;set;}
        public string BusinessWebsite { get; set;}
        public List<IFormFile> Photo { get; set;}
        public string AdminNotes{ get; set;}
        public List<IFormFile> Independent { get; set;}
        public List<IFormFile> hipaa { get; set;}
        public List<IFormFile> background { get; set;}
        public List<IFormFile> Non_disclosure { get; set;}
        public List<IFormFile> License { get; set;}
        public List<Physicianregion> phyReg { get; set;}
        public int status { get; set;}
        public string syncEmail { get; set;}
        public int regid { get; set;}
        public BitArray ind { get; set;}
        public BitArray hip { get; set;}
        public BitArray back { get; set;}
        public BitArray non_dis { get; set;}
        public BitArray lic { get; set;}
        enum statusName
        {
            Active,
            InActive,
            Pending,
        }
        public string findStatus(int status)
        {
            string sName = ((statusName)status).ToString();
            return sName;
        }
        public int Id {  get; set;}
        public List<int> selected { get; set; }
        public List<int> notselected { get; set; }

    }
}
