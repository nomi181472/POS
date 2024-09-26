using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class ItemImage:Base<string>
    {
        public virtual string? ItemId { get; set; }
        public virtual string? ItemCode { get; set; }
        public virtual string? ImagePath { get; set; }
        public virtual Items? Items { get; set; }
    }
}
