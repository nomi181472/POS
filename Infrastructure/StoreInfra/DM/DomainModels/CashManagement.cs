using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CashManagement : Base<String>
    {
        public string Currency { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }
    }
}
