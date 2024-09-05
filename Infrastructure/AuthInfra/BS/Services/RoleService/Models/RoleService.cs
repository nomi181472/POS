using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using DA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> AddRoleToUser(RequestAddRoleToUser request, string userId, CancellationToken cancellationToken)
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

            //await _unitOfWork.userRole.AddAsync(entity, userId, cancellationToken);
            //await _unitOfWork.CommitAsync(cancellationToken);

            return true;
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
