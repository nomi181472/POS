using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Request
{
    public class RequestAddActionsInRole
    {
        public string RoleId { get; set; }
        public List<string> Actions { get; set; }

    }
}
