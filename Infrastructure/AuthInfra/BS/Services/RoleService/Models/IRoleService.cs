using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models
{
    public interface IRoleService
    {
        Task<bool> AddRoleToUser(RequestAddRoleToUser request, string userId, CancellationToken cancellationToken);
        Task<List<ResponseGetAllUserRoles>> GetAllUserRoles(string userId,CancellationToken CancellationToken);
        Task<ResponseGetAllUserRoles> GetUserRoleById(string roleId, CancellationToken CancellationToken);
        Task<bool> DetachUserRole (string roleId,string userId, CancellationToken CancellationToken);
        Task<bool> DetachUserRoles(string[] roleId, string userId, CancellationToken CancellationToken);
       
    }
}
