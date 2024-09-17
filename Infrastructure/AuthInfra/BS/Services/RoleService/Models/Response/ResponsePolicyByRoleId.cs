using BS.Services.ActionsService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Response
{
    public class ResponsePolicyByRoleId: ResponseGetRole
    {
        public IEnumerable<ResponseGetActionWithDetails> Actions { get; set; }=new 
            List<ResponseGetActionWithDetails>();
        
    }
    
}
