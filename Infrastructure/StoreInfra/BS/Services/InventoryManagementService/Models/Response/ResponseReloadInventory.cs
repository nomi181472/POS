using InventoryService;
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
        public List<InventoryItemsAdded> InventoryItemsAdded { get; set; }   
    }

    public class InventoryItemsAdded
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Barcode { get; set; }
        public double Price { get; set; }
        public Item_Group Item_Group { get; set; }
        public List<string>? Categories { get; set; } = new List<string>();
        public Tax_Detail Tax { get; set; }
        public string ImagePath { get; set; }
    }

    public class Item_Group
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class Tax_Detail
    {
        public string TaxCode { get; set; }
        public double Percentage { get; set; }

    }
}
