using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class ProviderInfoViewModel
    {
        public BitArray BitArray { get; set; }
        public string ProviderName { get; set; }
        public int Status { get; set; }
        public bool
            IsActive
        { get; set; }
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
        public int id { get; set; }
        public int regid { get; set; }
        public List<Region> regions {  get; set; } 
    }
}
