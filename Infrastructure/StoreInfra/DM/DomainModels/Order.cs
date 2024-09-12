using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Order:Base<string>
    {
        public virtual string? CartId { get; set; }
        public virtual int TotalAmount { get; set; }
        public virtual int PaidAmount { get; set; }
        public virtual CustomerCart? CustomerCart { get; set; }
        public virtual ICollection<OrderSplitPayments>? OrderSplitPayments { get; set; }
    }
}
