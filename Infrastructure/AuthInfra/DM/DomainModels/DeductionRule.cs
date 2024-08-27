
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class DeductionRule : Base<string>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int StartTime { get; set; } // This variable represent the minutes 
        public int EndTime { get; set; } // This variable represent the minutes 
        public int Count { get; set; }
        public int Value { get; set; }
    }
}
