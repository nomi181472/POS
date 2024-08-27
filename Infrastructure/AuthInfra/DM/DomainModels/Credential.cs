using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Credential:Base<string>
    {
        
        public string PasswordSalt { get; set; }
        public string PasswordHah { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
