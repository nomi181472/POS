using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Response
{
    public class Carts
    {
        public virtual string? Id { get; set; }
        public virtual string? CustomerId { get; set; }
        public virtual string? CustomerName { get; set; }
        public virtual int? TotalItems { get; set; }
        public virtual int? TotalAmount { get; set; }
    }
}