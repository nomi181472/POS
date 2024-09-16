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
        public int ItemCode { get; set; }
        public string Barcode { get; set; }
        public double Price { get; set; } = 0;
        public string? GroupId { get; set; }
        public string? Category { get; set; }

        public virtual InventoryGroups? InventoryGroups { get; set; }
    }
}
