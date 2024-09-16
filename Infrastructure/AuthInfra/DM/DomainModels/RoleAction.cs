using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class RoleAction:Base<string>
    {
        public string? RoleId { get; set; }
        public string? ActionId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Actions? Actions { get; set; }
        public RoleAction(){}
        public RoleAction(string pId, string Createdby, DateTime pCreatedDate,string pRoleId,string pActionId) : base(pId, Createdby, pCreatedDate, true)
        {
            ActionId= pActionId;
            RoleId= pRoleId;
        }
    }
}
