using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Actions:Base<string>
    {
      
        public string Name { get; set; }
        public ICollection<RoleAction> RoleActions { get; set; } = new List<RoleAction>();
        public Actions(){}
        
        public Actions(string pId,string Createdby,DateTime pCreatedDate,string pName, List<RoleAction> pRoleActions):base(pId,Createdby,pCreatedDate)
        {
            Name = pName;
            RoleActions = pRoleActions;
        }
        public void UpdateActionName(string pName)
        {
            Name = pName;
        }


    }
}
