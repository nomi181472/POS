using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Model.Request
{
    public class RequestAddItemData
    {
        public List<ItemsDetail> Items { get; set; } = new List<ItemsDetail>();
    }

    public class ItemsDetail
    {
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemGrpCod { get; set; }
        public string ItemGrpName { get; set; }
        public string Barcode { get; set; } = "NA";
        public double Price { get; set; } = Double.MaxValue;
        public int QTY { get; set; }  = Int32.MaxValue;
        public List<string> Category { get; set; }
    }
}
