using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Models
{
    public class SchedulingViewModel
    {
        public List<Region> regions { get; set; }
        public int selectedregionid { get; set; }
        public int regionid { get; set; }
        public int providerid { get; set; }
        public DateTime shiftdate { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int repeatcount { get; set; }
        public int shiftid { get; set; }
        public List<Physician> active { get; set; }
        public List<Physician> inactive { get; set; }
        
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }
        public List<int> shiftIds { get; set; }
        public string regionname { get; set; }
        public string physicianname { get; set; }
        public DateOnly shifttoday { get; set; }
    }
}
