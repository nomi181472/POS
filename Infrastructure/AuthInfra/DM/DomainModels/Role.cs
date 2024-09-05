using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public class Role:Base<string>
    {
        public string Name { get;  set; }
        public ICollection<RoleAction> RoleActions { get; set; }=new List<RoleAction>();
        public Role(){} 
        public Role(string pId, string Createdby, DateTime pCreatedDate,string pName): base(pId, Createdby, pCreatedDate)
        {
                Name = pName;   
        }
    }

   
}
