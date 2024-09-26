using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Tax:Base<string>
    {
        public virtual string? ItemCode { get; set; }
        public virtual string? TaxCode { get; set; }
        public virtual int? Percentage { get; set; }
    }
}
