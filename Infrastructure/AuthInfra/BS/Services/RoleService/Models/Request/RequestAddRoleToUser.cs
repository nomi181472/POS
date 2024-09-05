using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Request
{
    public class RequestAddRoleToUser
    {
        public string UserId { get; set; }

        public string RoleId { get; set; } 
    }
}
