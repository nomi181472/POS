using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class WorkingProfile : Base<string>
    {
        public string? Code { get; set; }
        public string? Description { get; set; }

        public int? GraceTimeIn { get; set; }
        public int? GraceTimeOut { get; set; }
        public int? WorkingDays { get; set; }
        public int? WorkingHours { get; set; }

        //fk



        public virtual ICollection<AllowanceWorkingProfileManagement> AllowanceWorkingProfileManagements { get; set; } = new List<AllowanceWorkingProfileManagement>();
        public virtual ICollection<LeaveWorkingProfileManagement> LeaveWorkingProfileManagements { get; set; } = new
            List<LeaveWorkingProfileManagement>();

        public virtual string? FiscalYearId { get; set; }
        public virtual FiscalYear? FiscalYear { get; set; }

        public virtual ICollection<ShiftDeductionScheduler> ShiftDeductionScheduler { get; set; } = new List<ShiftDeductionScheduler>();

        public virtual ICollection<ShiftWorkingProfile> ShiftWorkingProfile { get; set; } = new List<ShiftWorkingProfile>();
    }
}
