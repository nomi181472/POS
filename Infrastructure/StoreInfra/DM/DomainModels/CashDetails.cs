using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CashDetails : Base<string>
    {
        public virtual string Currency {  get; set; }
        public virtual string Type { get; set; }
        public virtual int Quantity { get; set; }

        public virtual string CashSessionId { get; set; }
        public virtual CashSession CashSession { get; set; }
    }
}
