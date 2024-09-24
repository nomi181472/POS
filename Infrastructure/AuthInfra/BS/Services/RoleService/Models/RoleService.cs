﻿using BS.CustomExceptions.Common;
using BS.EnumsAndConstants.Constant;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using DA;
using DA.Common.CommonRoles;
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



        public async Task<bool> AddActionsInRole(RequestAddActionsInRole request, string userId, CancellationToken cancellationToken)
        {
            #region ArgumentNullValidations
            if (request.RoleId == null)
            {
                throw new ArgumentNullException("RoleId can't be empty");
            }

            if (request.Actions == null || !request.Actions.Any(action => !string.IsNullOrWhiteSpace(action)))
            {
                throw new ArgumentNullException("Action list cannot be empty.");
            }
            #endregion ArgumentNullValidations

            #region Fetch from DB
            var actionsFromReq = request.Actions.Select(x => x.ToLower());
            var actionsInDb = await _unitOfWork.action.GetAsync(cancellationToken,
                x => actionsFromReq.Contains(x.Name.ToLower()),
                includeProperties:
                $"{nameof(RoleAction)},"
            );

            if (!actionsInDb.Status)
            {
                throw new RecordNotFoundException(actionsInDb.Message);
            }
            #endregion Fetch from DB

            #region Adding new policy and assign it to the role
            // Set for quick lookups
            var actionsInDbSet = new HashSet<string>(actionsInDb.Data.Select(x => x.Name.ToLower()));

            // Filter out actions that already exist in the database
            var newActions = actionsFromReq.Where(x => !actionsInDbSet.Contains(x)).ToList();

            bool isUpdated = false;
            List<RoleAction> roleActions = new List<RoleAction>();

            // Adding new actions and assigning them to the role
            foreach (var item in newActions)
            {
                var action = new Actions(Guid.NewGuid().ToString(), userId, DateTime.UtcNow, item, item);
                var roleAction = new RoleAction(Guid.NewGuid().ToString(), userId, DateTime.UtcNow, request.RoleId, action.Id);

                _unitOfWork.action.Add(action, userId);
                _unitOfWork.roleAction.Add(roleAction, userId);

                isUpdated = true;
            }

            // Assigning existing actions to the role
            var oldActions = actionsInDb.Data.Where(x => !newActions.Contains(x.Name.ToLower()));
            foreach (var action in oldActions)
            {
                var alreadyAssignedSameAction = action.RoleAction;

                if (!alreadyAssignedSameAction.Any(x => x.IsRoleIdMatch(request.RoleId)))
                {
                    var roleAction = new RoleAction(Guid.NewGuid().ToString(), userId, DateTime.UtcNow, request.RoleId, action.Id);
                    _unitOfWork.roleAction.Add(roleAction, userId);
                    isUpdated = true; // Set the flag if any new role-actions are added
                }
            }
            #endregion Adding new policy and assign it to the role

            if (isUpdated)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }

            return false;
        }



        public async Task<bool> AddRole(RequestAddRole request, string userId, CancellationToken cancellationToken)
        {
            var roleExists = await _unitOfWork.role.AnyAsync(cancellationToken, r => r.Name.ToLower() == request.RoleName.ToLower() && r.IsActive);
            if (roleExists.Data)
            {
                throw new InvalidOperationException($"Role with name '{request.RoleName}' already exists.");
            }

            Role role = new Role(
                Guid.NewGuid().ToString(),
                Createdby: userId,
                pCreatedDate: DateTime.UtcNow,
                request.RoleName);
            role.UpdatedBy = userId;

            var result=await _unitOfWork.role.AddAsync(role, userId, cancellationToken);
            if (result.Result)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }
            throw new UnknownException(result.Message);
        }



        public async Task<bool> AddRoleToUser(RequestAddRoleToUser request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "The request cannot be null.");
            }

            var userExists = await _unitOfWork.user.GetByIdAsync(request.UserId, cancellationToken);
            if (userExists.Data == null)
            {
                throw new RecordNotFoundException($"User with ID {request.UserId} does not exist.");
            }
            if (userExists.Data.UserType == KDefinedRoles.SuperAdmin)
            {
                throw new InvalidOperationException("Can't assign role to superadmin");
            }

            var roleExists = await _unitOfWork.role.AnyAsync(cancellationToken, r => r.Id == request.RoleId);
            if (roleExists.Data == false)
            {
                throw new RecordNotFoundException($"Role with ID {request.RoleId} does not exist.");
            }

            var userHasRole = await _unitOfWork.userRole.AnyAsync(cancellationToken, ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId);
            if (userHasRole.Data)
            {
                throw new InvalidOperationException($"User with ID {request.UserId} already has the role with ID {request.RoleId}.");
            }

            var superAdminRole = await _unitOfWork.role.AnyAsync(cancellationToken, r => r.Id == request.RoleId && r.Name == "SuperAdmin");
            if (superAdminRole.Data == true)
            {
                throw new InvalidOperationException("Can't assign a SuperAdmin role to user");
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



        public async Task<bool> DeleteRole(RequestDeleteRole request, string userId, CancellationToken cancellationToken)
        {
            var getterResult = await _unitOfWork.role.GetByIdAsync(request.RoleId, cancellationToken);
            if (getterResult.Data == null)
            {
                throw new RecordNotFoundException("No role with such RoleId found");
            }

            var role = getterResult.Data;

            if (role.Name == KDefinedRoles.SuperAdmin)
            {
                throw new InvalidOperationException("Can't delete SuperAdmin role");
            }

            role.IsActive = false;
            role.UpdatedDate = DateTime.UtcNow;
            role.UpdatedBy = userId;

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



        public async Task<IEnumerable<ResponsePolicyByRoleId>> GetPoliciesByRoleId(string id, CancellationToken cancellationToken)
        {
            List<ResponsePolicyByRoleId> response = new List<ResponsePolicyByRoleId>();

            var result = await _unitOfWork.role.GetAsync(cancellationToken,
                 x => x.Id == id,
                 x => x.OrderByDescending(x => x.UpdatedDate),
                 includeProperties: $"{nameof(RoleAction)}," +
                                    $"{nameof(RoleAction)}.{nameof(Actions)}");

            if (result.Status && result.Data != null && result.Data.Any())
            {
                response.AddRange(result.Data.Select(item => item.ToSingleWithPolicyAction()));
                return response;
            }
            else
            {
                throw new RecordNotFoundException($"No policies found for Role ID: {id}");
            }
        }



        public async Task<ResponseGetRole> GetRole(string roleId, CancellationToken cancellationToken)
        {
            var result= await _unitOfWork.role.GetSingleAsync(cancellationToken,x=>x.Id == roleId && x.IsActive);
            if (result.Status)
            {
                if (result.Data == null)
                {
                    throw new RecordNotFoundException($"{roleId} not found.");
                }
                return result.Data.ToSingleResponseModel();
            }
            else
            {
                throw new UnknownException(result.Message);
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



        public bool IsRoleExist(string roleName)
        {
           var result=  _unitOfWork.role.Any(x=>x.Name==roleName && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }



        public bool IsRoleExistByRoleId(string[] roleIds)
        {
            return roleIds.All(id=> _unitOfWork.role.Any(x =>x.Id==id && x.IsActive).Data);
        }



        public bool IsRoleExistByRoleId(string roleId)
        {
            var result = _unitOfWork.role.Any(x => x.Id == roleId && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new RecordNotFoundException(result.Message);
            }
        }



        public async Task<List<ResponseGetRole>> ListRole( CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.role.GetAllAsync(cancellationToken);
            if (result.Status)
            {
                if (result.Data == null)
                {
                    return new List<ResponseGetRole>();

                }
                return result.Data.ToListResponseModel();
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }



        public async Task<bool> UpdateRole(RequestUpdateRole request, string userId, CancellationToken cancellationToken)
        {
            var getterResult = await _unitOfWork.role.GetByIdAsync(request.RoleId, cancellationToken);
            if (getterResult.Status == false)
            {
                throw new RecordNotFoundException("Could not find role with that ID");
            }
            var role = getterResult.Data;
            if(role.Name == KDefinedRoles.SuperAdmin)
            {
                throw new InvalidOperationException("Can not update SuperAdmin role");
            }

            role.Name = request.RoleName;
            role.UpdatedBy = userId;
            role.UpdatedDate = DateTime.UtcNow;


            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }



    }
}