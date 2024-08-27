
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Shift : Base<string>
    {
        public string Code { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int NumDays { get; set; } // 1 = Monday, 2 = Tuesday...
        public TimeOnly? TimeIn { get; set; }
        public TimeOnly? TimeOut { get; set; }

        public virtual ICollection<ShiftDeductionScheduler> ShiftDeductionScheduler { get; set; } = new List<ShiftDeductionScheduler>();

        public virtual ICollection<ShiftWorkingProfile> ShiftWorkingProfile { get; set; } = new List<ShiftWorkingProfile>();
    }
}


