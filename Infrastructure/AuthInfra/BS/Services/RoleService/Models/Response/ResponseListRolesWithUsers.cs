using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Response
{
    public class ResponseListRolesWithUsers
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<UsersInResponseListRolesWithUsers> Users { get; set; }
    }

    public class UsersInResponseListRolesWithUsers
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

}
