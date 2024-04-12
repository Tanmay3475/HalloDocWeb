using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class MonthWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }
        public List<Physician> physicians { get; set; }

    }
}
