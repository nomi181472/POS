using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Location: Base<string>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }    
        public string Province { get; set; }
    }
}
