using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class PartnerViewModel
    {
        public string Name { get; set; }
        public int Pro { get; set; }
        public string FaxNum { get; set; }
        public string Num { get; set; }
        public string Email { get; set; }
        public string BusinessCon { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public List<Healthprofessionaltype> Healthprofessionaltypes { get; set; }
    }
}
