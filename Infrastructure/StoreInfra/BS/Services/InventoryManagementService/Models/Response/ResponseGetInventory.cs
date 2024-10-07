using DM.DomainModels;
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
        public virtual int? Quantity { get; set; }
        public virtual ItemGroupDetails? ItemGroupDetails { get; set; }
        public virtual ItemTaxDetails? ItemTaxDetails { get; set; }
    }

    public class ItemGroupDetails
    {
        public virtual string GroupId { get; set; }
        public virtual string GroupCode { get; set; }
        public virtual string GroupName { get; set; }
    }

    public class ItemTaxDetails
    {
        public virtual string TaxId { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual double Percentage { get; set; }
    }

}
