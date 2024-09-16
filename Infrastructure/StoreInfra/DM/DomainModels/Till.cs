using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Till:Base<string>
    {
        public virtual string? Name { get; set; }
        public virtual string? Description { get; set; }
        public virtual ICollection<CustomerCart>? Carts { get; set; }
    }
}
