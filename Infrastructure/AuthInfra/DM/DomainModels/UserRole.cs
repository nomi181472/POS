using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class UserRole:Base<string>
    {
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public UserRole(string? pUserId,string pId, string Createdby, DateTime pCreatedDate): base(pId, Createdby, pCreatedDate)
        {
            UserId = pUserId;
        }
    }
}
