using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CustomerCart : Base<string>
    {
        public virtual string? CustomerId { get; set; }
        public virtual bool IsConvertedToSale { get; set; } = false;
        public virtual string? TillId { get; set; }
        public virtual Till? Till { get; set; }
        public virtual CustomerManagement? CustomerManagement { get; set; }
        public virtual ICollection<CustomerCartItems>? CustomerCartItems { get; set; }
        public virtual ICollection<Order>? Order { get; set; }
    }
}