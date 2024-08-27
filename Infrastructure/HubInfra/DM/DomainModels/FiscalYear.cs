using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class FiscalYear : Base<string>
    {
        public string Name { get; set; }
        public string type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<WorkingProfile> WorkingProfiles { get; set; } = new
            List<WorkingProfile>();


    }
}
