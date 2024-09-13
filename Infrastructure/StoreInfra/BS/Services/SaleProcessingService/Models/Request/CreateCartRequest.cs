using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Request
{
    public class CreateCartRequest
    {
        public virtual string? CustomerId {  get; set; }
        public virtual bool IsConvertedToSale {  get; set; }
        public virtual string? TillId {  get; set; }
    }
}
