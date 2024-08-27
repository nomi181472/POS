using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Role:Base<string>
    {
        
        public string Name { get; set; }
        public ICollection<RolePolicy> RolePolicies { get; set; }
    }

   
}
