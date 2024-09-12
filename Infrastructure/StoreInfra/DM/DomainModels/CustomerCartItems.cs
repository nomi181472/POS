using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CustomerCartItems : Base<string>
    {
        public virtual string CartId { get; set; }
        public virtual string ItemId { get; set; }
        public virtual CustomerCart? CustomerCart {get; set;}
    }
}
