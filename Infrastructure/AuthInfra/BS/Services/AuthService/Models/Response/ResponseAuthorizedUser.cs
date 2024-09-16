using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService.Models.Response
{
    public class ResponseAuthorizedUser
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string UserId { get; set; }
        public string UserType { get;set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string[] RoleAndActions { get; set; } = new string[0];
    }
}
