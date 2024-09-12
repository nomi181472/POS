using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Request
{
    public class AddItemsToCartRequest
    {
        public virtual List<CartItemsList>? Items { get; set; }
        public virtual string? CartId { get; set; }
    }

    public class CartItemsList
    {
        public virtual string? ItemId { get; set; }
        public virtual int? Quantity { get; set; }
    }
}
