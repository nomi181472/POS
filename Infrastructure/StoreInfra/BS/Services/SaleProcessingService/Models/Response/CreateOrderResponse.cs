using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Response
{
    public class CreateOrderResponse
    {
        public virtual int TotalAmount { get; set; }
        public virtual int PaidAmount { get; set; }
        public virtual bool IsPaid { get; set; }
        public virtual bool Success { get; set; }
        public virtual string? Message {  get; set; }
    }
}
