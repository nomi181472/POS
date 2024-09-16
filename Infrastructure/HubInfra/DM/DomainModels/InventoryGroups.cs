using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class InventoryGroups : Base<String>
    {
        public string Name { get; set; }
        public int Code { get; set; }

        public virtual ICollection<InventoryItems> InventoryItems { get; set; }
    }
}
