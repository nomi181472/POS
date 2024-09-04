using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models
{
    public class RoleService : IRoleService
    {
        public Task<bool> AddRoleToUser(RequestAddRoleToUser request, string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DetachUserRole(string roleId, string userId, CancellationToken CancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DetachUserRoles(string[] roleId, string userId, CancellationToken CancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResponseGetAllUserRoles>> GetAllUserRoles(string userId, CancellationToken CancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseGetAllUserRoles> GetUserRoleById(string roleId, CancellationToken CancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
