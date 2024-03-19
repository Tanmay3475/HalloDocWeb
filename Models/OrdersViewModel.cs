using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class OrdersViewModel
    {
       public List<Healthprofessionaltype> healthprofessionaltypes {  get; set; }
        public int profession      {  get; set; }
        public string business         {  get; set; }
        public string business_contact { get; set;}
        public string business_email   { get; set;}
        public string prescription     { get; set;}
        public string fax_number      { get; set;}
        public int    no_of_refills   { get; set;}

    }
}
