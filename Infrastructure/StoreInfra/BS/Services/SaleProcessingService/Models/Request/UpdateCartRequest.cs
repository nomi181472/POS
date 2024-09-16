using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Request
{
    public class UpdateCartRequest
    {
        public virtual string? CartId { get; set; }
        public virtual string? CustomerId { get; set; }
        public virtual bool IsConvertedToSale { get; set; }
    }
}
