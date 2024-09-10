using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Response
{
    public class ResposeGetRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreatedBy { get; set; }   
        public string UpdatedBy { get; set;}
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
