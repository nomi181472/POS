using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Response
{
    public class ResponseUserDetailsWithRoleAndPolicies:ResponseGetUser
    {
        public ResponseUserRefreshTokens RefreshTokensDetails { get; set; }
        public IEnumerable<ResponseUserRoles> RoleDetails { get; set; }


    }
}
