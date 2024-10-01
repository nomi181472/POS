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
        bool IsRoleExist(string roleName);
        bool IsRoleExistByRoleId(string roleId);
        bool IsRoleExistByRoleId(string[] roleId);
        Task<bool> AddRole(RequestAddRole request, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteRole(RequestDeleteRole request, string userId, CancellationToken cancellationToken);
        Task<bool> UpdateRole(RequestUpdateRole request, string userId, CancellationToken cancellationToken);
        Task<ResponseGetRole> GetRole(string roleId, CancellationToken cancellationToken);
        Task<List<ResponseGetRole>> ListRole( CancellationToken cancellationToken);
        Task<bool> AddActionsInRole(RequestAddActionsInRole request, string v, CancellationToken cancellationToken);
        Task<IEnumerable<ResponsePolicyByRoleId>> GetPoliciesByRoleId(string id, CancellationToken cancellationToken);
        Task<IEnumerable<ResponseListRolesWithUsers>> ListRolesWithUsers(CancellationToken cancellationToken);
    }
}
