
using BS.Services.RoleService.Models;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using BS.Services.UserService.Models.Response;
using DM.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Metadata;

namespace BS.Services.UserService.Models
{
    public static class CRTUser
    {

        public static User ToDomainModel(this RequestAddUser request, string updatedBy, DateTime now, string ph, string ps, string userId)
        {
            Credential credential = new Credential(
                pId: Guid.NewGuid().ToString(),
                Createdby: updatedBy,
                pPasswordHash: ph,
                pPasswordSalt: ps,
                pCreatedDate: now,
                pUserId: userId
                );
            User User = new User(
                userId,
                Createdby: userId,
                pCreatedDate: now,
                pName: request.Name,
                pCredential: credential,
                pEmail: request.Email,
                pUserType: request.UserType,
                pUserRoles: request.RoleIds.Select(x => new UserRole(
                    userId,
                    pRoleId: x,
                    pCreatedDate: now,
                    pId: Guid.NewGuid().ToString(),
                    Createdby: updatedBy


                    )
                    
                ).ToList()
                ,
                pRefreshToken:new RefreshToken(
                    Guid.NewGuid().ToString(),
                    updatedBy,
                    now,
                    Guid.NewGuid().ToString(),
                    request.IsRefreshTokenRevokable,
                    request.ExpiryDate,
                    userId
                    )

                );
            
            return User;
        }

        public static ResponseGetUser ToSingleResponseModel(this User response)
        {
            return new ResponseGetUser()
            {
                UserId = response.Id,
                Name = response.Name,
                Email= response.Email,
               
                CreatedBy = response.CreatedBy,
                CreatedDate = response.CreatedDate,
                IsActive = response.IsActive,
                UpdatedBy = response.UpdatedBy,
                UpdatedDate = response.UpdatedDate



            };
        }
        public static ResponseUserRefreshTokens ToResponseRefreshTokenDetails(this RefreshToken response)
        {
            return new ResponseUserRefreshTokens()
            {
                CreatedBy = response.CreatedBy,
                CreatedDate = response.CreatedDate,
                IsActive = response.IsActive,
                UpdatedBy = response.UpdatedBy,
                UpdatedDate = response.UpdatedDate,
                ExpireyDate = response.ExpireyDate,
                RevokeAble = response.RevokeAble,
                IsArchived = response.IsArchived,
                UserId = response.UserId,
                RefreshTokenId = response.Id,
            };
        }
        public static ResponseUserRoles ToResponse(this UserRole response)
        {
            Role role = response.Role??new Role();
            return new ResponseUserRoles()
            {
                CreatedBy = role.CreatedBy,
                CreatedDate = role.CreatedDate,
                IsActive = role.IsActive,
                UpdatedBy = role.UpdatedBy,
                UpdatedDate = role.UpdatedDate,
                RoleId = role.Id,
                RoleName = role.Name,
                Actions = role.RoleAction.Select(x => x.ToResponsePolicy()),
                UserRoleAssignDate = response.CreatedDate,
            };
        }
        public static ResponseUserDetailsWithRoleAndPolicies ToResponseUserDetailsWithActions(this User response)
        {
            return new ResponseUserDetailsWithRoleAndPolicies()
            {
                UserId = response.Id,
                Name = response.Name,
                Email = response.Email,

                CreatedBy = response.CreatedBy,
                CreatedDate = response.CreatedDate,
                IsActive = response.IsActive,
                UpdatedBy = response.UpdatedBy,
                UpdatedDate = response.UpdatedDate,
                RefreshTokensDetails = response?.RefreshToken?.ToResponseRefreshTokenDetails() ?? new ResponseUserRefreshTokens(),
                RoleDetails = response?.UserRole?.Select(x => x.ToResponse()) ?? new List<ResponseUserRoles>()



            };
        }

        public static List<ResponseGetUser> ToListResponseModel( this IEnumerable<User> response)
    {
        return response.Select(x => x.ToSingleResponseModel()).ToList();
        
    }
}
}
