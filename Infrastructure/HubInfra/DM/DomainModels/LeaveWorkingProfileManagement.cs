using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class LeaveWorkingProfileManagement : Base<string>
    {

        //fk
        public virtual string? LeaveId { get; set; }
        public virtual string? WorkingProfileId { get; set; }

        public Leave? Leave { get; set; }
        public WorkingProfile? WorkingProfile { get; set; }

    }
}
