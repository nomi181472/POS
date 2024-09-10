using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Request
{
    public class RequestUpdateUser
    {
        
        public string UserId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
       
    }
}
