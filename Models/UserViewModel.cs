using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string last { get; set; }
        public string mobile { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string street { get; set; }
        public DateTime dob { get; set; }


    }
}
