using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class User:Base<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }   
        public virtual Credential? Credential { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }=new List<UserRole>();

    }
}
