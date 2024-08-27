using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class RolePolicy:Base<string>
    {
        public string? RoleId { get; set; }
        public string? PolicyId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Policy? Policy { get; set; }
        
      
    }
}
