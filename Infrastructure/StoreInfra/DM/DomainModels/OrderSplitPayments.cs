using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class OrderSplitPayments : Base<string>
    {
        public virtual string? OrderId { get; set; }
        public virtual string? PaymentMethodId {  get; set; }
        public virtual int Amount { get; set; }
        public virtual Order? Order { get; set; }
        public virtual PaymentMethods? PaymentMethods { get; set; }
        public virtual ICollection<PaymentTransactions>? PaymentTransactions { get; set; }
    }
}
