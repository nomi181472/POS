using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Leave : Base<string>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual int? CompanyId { get; set; }

        public virtual ICollection<LeaveWorkingProfileManagement> LeaveWorkingProfileManagements { get; set; } = new List<LeaveWorkingProfileManagement>();
    }
}
