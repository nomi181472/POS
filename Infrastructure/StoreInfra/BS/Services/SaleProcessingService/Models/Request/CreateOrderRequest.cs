using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.SaleProcessingService.Models.Request
{
    public class CreateOrderRequest
    {
        public virtual string? CartId { get; set; }
        public virtual List<SplitPaymentRequest>? OrderSplitPayments { get; set; }
        public virtual int TotalAmount { get; set; }

        public virtual double DownPayment { get; set; }
        public virtual bool IsLayaway {  get; set; } = false;
        public virtual bool IsPartial {  get; set; } = false;
    }


    public class SplitPaymentRequest
    {
        public virtual string? PaymentMethodId { get; set; }
        public virtual int PaidAmount { get; set; }
        public virtual string? CardNumber { get; set; }
        public virtual string? Cvv {  get; set; }
        public virtual string? Mm {  get; set; }
        public virtual string? Yy { get; set; }
    }
}
