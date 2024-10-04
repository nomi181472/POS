using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models.Response
{
    public class ResponseGetRbacMatrix
    {
        public List<ActionDto> ActionsInFeature { get; set; }
        public List<RoleDto> AllRoles { get; set; }
        public List<RoleActionDto> ActionsAssociatedWithRole { get; set; }
    }

    public class ActionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleActionDto
    {
        public string RoleId { get; set; }
        public string ActionId { get; set; }
    }
}
