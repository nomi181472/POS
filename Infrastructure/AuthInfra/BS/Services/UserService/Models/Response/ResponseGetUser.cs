using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models.Response
{
    public class ResponseGetUser
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string CreatedBy { get; set; }   
        public string UpdatedBy { get; set;}
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
