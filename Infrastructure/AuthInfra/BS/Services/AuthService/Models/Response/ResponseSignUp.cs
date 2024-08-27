using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService.Models.Response
{
    public class ResponseSignUp
    {
        public string UserId { get; set; }
        public string[] RoleIds { get; set; } = new string[0];
    }
}
