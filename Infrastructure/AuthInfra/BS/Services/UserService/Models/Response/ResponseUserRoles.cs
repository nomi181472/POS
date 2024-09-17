using BS.Services.RoleService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Response
{
    public class ResponseUserRoles:ResponsePolicyByRoleId
    {
        public DateTime UserRoleAssignDate { get; set; }
    }
}
