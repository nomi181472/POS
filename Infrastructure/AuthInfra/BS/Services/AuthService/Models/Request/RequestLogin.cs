using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService.Models.Request
{
    public class RequestLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
