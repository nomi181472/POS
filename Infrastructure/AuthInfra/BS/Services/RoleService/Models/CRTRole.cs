using BS.Services.ActionsService.Models.Response;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
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
        public static UserRole ToDomain(this RequestAddRoleToUser request, string userId)
        {
            DateTime createdDate = DateTime.Now;
            string Id = Guid.NewGuid().ToString();
            return new UserRole(request.UserId, request.RoleId, Id, userId, createdDate);
        }
        public static ResponseGetRole ToSingleResponseModel(this Role response)
        {
            return new ResponseGetRole()
            {
                RoleId = response.Id,
                RoleName = response.Name,
                CreatedBy = response.CreatedBy,
                CreatedDate = response.CreatedDate,
                IsActive = response.IsActive,
                UpdatedBy = response.UpdatedBy,
                UpdatedDate = response.UpdatedDate



            };
        }
        public static ResponseGetActionWithDetails ToResponsePolicy(this RoleAction response)
        {
            Actions action = response?.Actions?? new Actions();
            return new ResponseGetActionWithDetails()
            {
                PolicyId = action.Id,
                ActionName = action.Name,
                CreatedBy = action.CreatedBy,
                CreatedDate = action.CreatedDate,
                IsActive = action.IsActive,
                UpdatedBy = action.UpdatedBy,
                UpdatedDate = action.UpdatedDate,
            };
        }
        public static ResponsePolicyByRoleId ToSingleWithPolicyAction(this Role response)
        {
            return new ResponsePolicyByRoleId()
            {
                RoleId = response.Id,
                RoleName = response.Name,
                CreatedBy = response.CreatedBy,
                CreatedDate = response.CreatedDate,
                IsActive = response.IsActive,
                UpdatedBy = response.UpdatedBy,
                UpdatedDate = response.UpdatedDate,
                Actions=response.RoleAction.Select(x=>x.ToResponsePolicy())



            };
        }

        public static List<ResponseGetRole> ToListResponseModel( this IEnumerable<Role> response)
    {
        return response.Select(x => x.ToSingleResponseModel()).ToList();
        
    }
}
}
