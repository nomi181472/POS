using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class ShiftDeductionScheduler : Base<string>
    {
        public virtual string? ShiftId { get; set; }

        public virtual string? DeductionId { get; set; }
        public virtual string? WorkingProfileId { get; set; }

        public virtual Shift? Shift { get; set; }

        public virtual Deduction? Deduction { get; set; }

        public WorkingProfile? WorkingProfile { get; set; }
    }
}
