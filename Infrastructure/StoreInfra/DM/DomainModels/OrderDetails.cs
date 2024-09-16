using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class OrderDetails:Base<string>
    {
        public virtual string? ItemName { get; set; }
        public virtual int Price {get; set;}
        public virtual int Quantity {get; set;}
    }
}
