using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class PaymentTransactions:Base<string>
    {
        public virtual string? TaxId { get; set; }
        public virtual string? Amount { get; set; }
        public virtual string? SplitPaymentId { get; set; }
        public virtual OrderSplitPayments? OrderSplitPayments { get; set; }
    }
}
