using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class CashSession : Base<string>
    {
        public virtual string TillId { get; set; }
        public virtual string UserId { get; set; }
        public virtual double TotalAmount { get; set; }

        public virtual ICollection<CashDetails> cashDetails { get; set; }
    }
}
