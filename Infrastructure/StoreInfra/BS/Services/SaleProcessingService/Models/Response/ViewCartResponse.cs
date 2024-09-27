using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Response
{
    public class ViewCartResponse
    {
        public virtual string? CustomerId { get; set; }
        public virtual List<CartItems>? CartItems { get; set; } = new List<CartItems>();
    }

    public class CartItems
    {
        public virtual string? ItemId { get; set; }
        public virtual string? ItemCode { get; set; }
        public virtual string? ItemName { get; set; }
        public virtual int? Price { get; set; }
        public virtual int? Quantity { get; set; }
    }
}
