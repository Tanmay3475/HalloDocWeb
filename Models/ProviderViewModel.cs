using Microsoft.AspNetCore.Http;
using System;
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
        public IFormFile Photo { get; set;}
        public string AdminNotes{ get; set;}
        public IFormFile Independent { get; set;}
        public IFormFile hipaa { get; set;}
        public IFormFile background { get; set;}
        public IFormFile Non_disclosure { get; set;}
        public IFormFile License { get; set;}
    }
}
