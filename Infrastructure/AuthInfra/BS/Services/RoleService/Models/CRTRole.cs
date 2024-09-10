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
        public static ResposeGetRole ToSingleResponseModel(this Role response)
        {
            return new ResposeGetRole()
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
    
    public static List<ResposeGetRole> ToListResponseModel( this IEnumerable<Role> response)
    {
        return response.Select(x => x.ToSingleResponseModel()).ToList();
        
    }
}
}
