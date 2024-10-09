using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class StorageLocation: Base<string>
    {
        public string ERPCode { get; set; }
        public string ERPName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string LocCode { get; set; }
        public string CompanyCode { get; set; }
        public string Address { get; set; }

        public Store Store { get; set; }
    }
}
