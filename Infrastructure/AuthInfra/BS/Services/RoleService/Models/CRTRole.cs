using BS.Services.RoleService.Models.Request;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace BS.Services.RoleService.Models
{
    public static class CRTRole
    {
        public static Role ToDomain(this RequestAddRole request, string userId)
        {
            DateTime createdDate = DateTime.Now;
            string Id = Guid.NewGuid().ToString();
            return new Role(Id, userId, createdDate, request.Name);
        }

        public static UserRole ToDomain(this RequestAddRoleToUser request, string userId)
        {
            DateTime createdDate = DateTime.Now;
            string Id = Guid.NewGuid().ToString();
            return new UserRole(request.UserId, request.RoleId, Id, userId, createdDate);
        }
    }
}
