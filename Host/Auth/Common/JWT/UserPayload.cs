using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Common.JWT
{
    public class UserPayload
    {
       public required string Id {get;set;} 
       public string[] RoleIds { get;set;}=new string[0];
    }
}