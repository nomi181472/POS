using BS.Services.ActionsService.Models.Request;
using BS.Services.RoleService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.ActionsService.Models
{
    public static class CRTAction
    {
        public static Actions ToDomain(this RequestAddAction request, string userId)
        {
            DateTime createdDate = DateTime.Now;
            string Id = Guid.NewGuid().ToString();
            List<RoleAction> emptyListOfRoleActions = new List<RoleAction>();
            return new Actions(Id, userId, createdDate, request.Name, emptyListOfRoleActions);
        }
    }
}
