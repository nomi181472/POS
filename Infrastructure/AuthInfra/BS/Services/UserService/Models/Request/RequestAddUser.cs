using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Request
{
    public class RequestAddUser
    {
       

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        
        public string ConfirmedPassword { get; set; }
        public bool IsRefreshTokenRevokable { get; set; } = true;  
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
        public List<string> RoleIds { get; set; } = new List<string>();
        public string UserType { get;  set; }
    }
}
