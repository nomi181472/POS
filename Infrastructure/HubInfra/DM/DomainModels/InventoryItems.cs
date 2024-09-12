using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class InventoryItems: Base<String>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
