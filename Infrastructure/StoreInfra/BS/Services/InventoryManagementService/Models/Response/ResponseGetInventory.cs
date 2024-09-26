using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.InventoryManagementService.Models.Response
{
    public class ResponseGetInventory
    {
        public virtual string? ItemId { get; set; }
        public virtual string? ItemCode { get; set; }
        public virtual string? ItemName { get; set; }
        public virtual int? Price { get; set; }
        public virtual string? ImagePath { get; set; }
    }
}
