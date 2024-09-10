using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Request
{
    public class RequestAddUser
    {
        public string UserId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        
        public string ConfirmedPassword { get; set; }
        public List<string> RoleIds { get; set; }
        public string UserType { get;  set; }
    }
}
