using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Items:Base<string>
    {
        public virtual string? ItemCode {  get; set; }
        public virtual string? ItemName { get; set; }
        public virtual int? ItemGroupCode { get; set; }
        public virtual string? PurchaseItem { get; set; }
        public virtual string? SellItem { get; set; }
        public virtual string? InventoryItem { get; set; }
        public virtual int? UgpEntry { get; set; }
        public virtual string? BuyUom { get; set; }
        public virtual string? SellUom { get; set; }
        public virtual string? InvUom { get; set; }
        public virtual bool IsSerial { get; set; }
        public virtual bool IsBatch { get; set; }
        public virtual string? TaxType { get; set; }
        public virtual string? TaxCode { get; set; }
        public virtual int? Price { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual ICollection<ItemImage>? ItemImages { get; set; }
        public virtual ICollection<Tax>? Taxes { get; set; }
        public virtual ICollection<CustomerCartItems>? CustomerCartItems { get; set; }
    }
}
