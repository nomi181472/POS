using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Models.Response
{
    public class ResponseReloadInventory
    {
        public List<InventoryItems> InventoryItems { get; set; }   
    }

    public class InventoryItems
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public double Price { get; set; }
        public ItemGroup ItemGroup { get; set; }
        public List<string>? Categories { get; set; } = new List<string>();
    }

    public class ItemGroup
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
