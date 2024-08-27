using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Deduction : Base<string>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public virtual ICollection<ShiftDeductionScheduler> ShiftDeductionScheduler { get; set; } = new
            List<ShiftDeductionScheduler>();

    }
}
