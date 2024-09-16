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
        public string Tags { get; set; }
        public ICollection<RoleAction> RoleAction { get; set; } = new List<RoleAction>();
        public Actions(){}
        
        public Actions(string pId,string Createdby,DateTime pCreatedDate,string pName, string pTags, List<RoleAction> pRoleActions):base(pId,Createdby,pCreatedDate, true)
        {
            Name = pName;
            Tags = pTags;
            RoleAction = pRoleActions;
        }
        public Actions(string pId,string Createdby,DateTime pCreatedDate,string pName, string pTags):base(pId,Createdby,pCreatedDate, true)
        {
            Name = pName;
            Tags = pTags;
            
        }
        public void UpdateActionName(string pName)
        {
            Name = pName;
        }
        public void UpdateActionTag(string pTags)
        {
            Tags = pTags;
        }
        public string GetActionTag()
        {
            return Tags;
        }
    }
}
