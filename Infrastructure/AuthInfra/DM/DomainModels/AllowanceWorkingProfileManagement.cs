using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class AllowanceWorkingProfileManagement : Base<string>
    {
        //fk
        public virtual string? AllownaceId { get; set; }
        public virtual string? WorkingProfileId { get; set; }

        public virtual Allowance? Allowance { get; set; }

        public virtual WorkingProfile? WorkingProfile { get; set; }

    }
}
