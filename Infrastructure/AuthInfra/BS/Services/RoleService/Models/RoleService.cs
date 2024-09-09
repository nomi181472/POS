using BS.CustomExceptions.Common;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using DA;
using DM.DomainModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BS.Services.RoleService.Models
{
    public class RoleService : IRoleService
    {
        IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddRole(RequestAddRole request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var entity = request.ToDomain(userId);

            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.role.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> AddRoleToUser(RequestAddRoleToUser request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var userExists = await _unitOfWork.user.AnyAsync(cancellationToken, u => u.Id == request.UserId);
            if(userExists.Data == false)
            {
                throw new RecordNotFoundException($"User with ID {request.UserId} does not exist.");
            }

            var roleExists = await _unitOfWork.role.AnyAsync(cancellationToken, r => r.Id == request.RoleId);
            if (roleExists.Data == false)
            {
                throw new RecordNotFoundException($"Role with ID {request.RoleId} does not exist.");
            }

            var entity = request.ToDomain(userId);

            if (entity == null)
            {
                throw new ArgumentException("The request is invalid and could not be converted to a domain entity.", nameof(request));
            }

            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            entity.IsArchived = false;
            entity.IsActive = true;

            await _unitOfWork.userRole.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DetachUserRole(string roleId, string userId, CancellationToken cancellationToken)
        {
            if (roleId == null)
            {
                throw new ArgumentNullException("roleId can not be null or empty");
            }

            var setterResult = await _unitOfWork.userRole.UpdateOnConditionAsync(
            // 1st param: matching condition
            x => x.IsActive == true && x.RoleId == roleId,
            // 2nd param: set the updated value
            x => x.SetProperty((Func<UserRole, string?>)(y => y.RoleId), (string?)null)
                  .SetProperty((Func<UserRole, string>)(y => y.UpdatedBy), userId)
                  .SetProperty((Func<UserRole, DateTime>)(y => y.UpdatedDate), DateTime.UtcNow)
                  , cancellationToken);

            if (setterResult == null)
            {
                throw new InvalidOperationException("The update operation did not return a result.");
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DetachUserRoles(string[] roleId, string userId, CancellationToken cancellationToken)
        {
            if (roleId == null || roleId.Length == 0)
            {
                throw new ArgumentException("Role IDs cannot be null or empty.", nameof(roleId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var result = await _unitOfWork.userRole.GetAllAsync(cancellationToken);

            if (result == null || !result.Status || result.Data == null)
            {
                throw new InvalidOperationException("Could not retrieve UserRoles list");
            }

            var affectedRoles = result.Data.ToList();

            foreach (var role in affectedRoles)
            {
                role.IsActive = false;
                role.UpdatedBy = userId;
                role.UpdatedDate = DateTime.UtcNow;
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }

        public async Task<List<ResponseGetAllUserRoles>> GetAllUserRoles(string userId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.userRole.GetAllAsync(cancellationToken);
            List<ResponseGetAllUserRoles> response = new List<ResponseGetAllUserRoles>();

            if (result.Status)
            {
                foreach(var record in result.Data)
                {
                    if(record.UserId != null && record.RoleId != null && record.IsActive == true)
                    {
                        response.Add(new ResponseGetAllUserRoles()
                        {
                            UserId = record.UserId,
                            RoleId = record.RoleId
                        });
                    }
                }
                return response;
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve UserRoles data.");
            }
        }

        public async Task<ResponseGetAllUserRoles> GetUserRoleById(string roleId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new ArgumentException("Role ID cannot be null or empty.", nameof(roleId));
            }

            var userRoles = await _unitOfWork.userRole.GetAsync(cancellationToken, x => x.RoleId == roleId && x.IsActive);

            if (userRoles == null)
            {
                throw new RecordNotFoundException($"User role with ID {roleId} not found.");
            }

            var userRole = userRoles.Data.FirstOrDefault();

            if (userRole != null && userRole.UserId != null && userRole.RoleId != null)
            {
                return new ResponseGetAllUserRoles()
                {
                    UserId = userRole.UserId,
                    RoleId = userRole.RoleId
                };
            }
            else
            {
                throw new RecordNotFoundException("UserId or RoleId not found");
            }
        }
    }
}