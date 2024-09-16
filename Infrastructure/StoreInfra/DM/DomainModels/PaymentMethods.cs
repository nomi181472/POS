using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class PaymentMethods:Base<string>
    {
        public virtual string? Name { get; set; }
        public virtual ICollection<OrderSplitPayments>? OrderSplitPayments { get; set; }
    }
}
