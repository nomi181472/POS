using BS.CustomExceptions.Common;
using DA;
using DM.DomainModels;

using BS.Services.RoleService.Models.Request;
using BS.Services.UserService.Models.Response;
using BS.Services.UserService.Models.Request;
using Helpers.CustomExceptionThrower;
using Helpers.StringsExtension;
using DA.Common.CommonRoles;

namespace BS.Services.UserService.Models
{
    public class UserService : IUserService
    {
        IUnitOfWork _uot;
        public UserService(IUnitOfWork unitOfWork)
        {
            _uot = unitOfWork;
        }



        public async Task<bool> AddUser(RequestAddUser request, string updatedBy, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new InvalidDataException("request can't be null");
            }
            if (request.RoleIds.Count() == 0)
            {
                throw new InvalidDataException("Atleast one role required");
            }
            if (request.RoleIds.Count != request.RoleIds.Distinct().Count())
            {
                throw new InvalidDataException("Duplicate RoleIds are not allowed.");
            }

            var now = DateTime.UtcNow;
           
            var hAndS = PasswordHelper.HashPassword(request.Password);
            string ph = hAndS.hash;
            string ps = hAndS.salt;
            string userId = Guid.NewGuid().ToString();
            User User = request.ToDomainModel(updatedBy, now, ph, ps, userId);

            var superAdminRoleExists = await _uot.role.AnyAsync(cancellationToken, r => request.RoleIds.Contains(r.Id) && r.Name==KDefinedRoles.SuperAdmin);
            if (superAdminRoleExists.Data)
            {
                throw new InvalidOperationException("Can't assign SuperAdmin role.");
            }

            var existingUserResult = await _uot.user.GetAsync(cancellationToken, u => u.Email == request.Email);
            var existingUser = existingUserResult.Data.FirstOrDefault();
            if (existingUser != null)
            {
                throw new RecordAlreadyExistException("Email already registered");
            }

            if (request.ConfirmedPassword != request.Password)
            {
                throw new InvalidDataException("Passwords don't match");
            }

            var result = await _uot.user.AddAsync(User, userId, cancellationToken);



            ArgumentFalseException.ThrowIfFalse(result.Result, result.Message);

            await _uot.CommitAsync(cancellationToken);
            return true;
        }


        
        public async Task<bool> DeleteUser(RequestDeleteUser request, string userId, CancellationToken cancellationToken)
        {
            var getterResult = await _uot.user.GetByIdAsync(request.UserId, cancellationToken);
            if(getterResult.Data == null)
            {
                throw new RecordNotFoundException("No user with such ID found");
            }

            var userToUpdate = getterResult.Data;

            if (userToUpdate.UserType == KDefinedRoles.SuperAdmin)
            {
                throw new InvalidOperationException("Can't delete SuperAdmin user");
            }

            var existingUserRoles = await _uot.userRole.GetAsync(cancellationToken, x => x.UserId == userToUpdate.Id);
            var userRolesToDelete = existingUserRoles.Data;
            if (userRolesToDelete.Count() != 0)
            {
                foreach (var role in userRolesToDelete)
                {
                    role.IsActive = false;
                }
            }

            userToUpdate.IsActive = false;
            userToUpdate.UpdatedBy = userId;
            userToUpdate.UpdatedDate = DateTime.UtcNow;

            await _uot.CommitAsync(cancellationToken);
            return true;
        }



        public async Task<ResponseGetUser> GetUser(string UserId, CancellationToken cancellationToken)
        {
            if (UserId == null)
            {
                throw new ArgumentNullException("UserId can't be null");
            }
            var result= await _uot.user.GetSingleAsync(cancellationToken,x=>x.Id == UserId && x.IsActive);
            if (result.Status)
            {
                if (result.Data == null)
                {
                    throw new RecordNotFoundException($"{UserId} not found.");

                }
                return result.Data.ToSingleResponseModel();
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }



        public async Task<ResponseUserDetailsWithRoleAndPolicies> GetUserDetailsWithActions(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Id can't be null");
            }
            #region Getting user
            var userData = await _uot.user.GetSingleAsync(cancellationToken, u => u.Id == id && u.UserRole.All(ur => ur.Role.RoleAction.All(ra => ra.Actions.IsActive)),
                                                        $"{nameof(Credential)}," +
                                                        $"{nameof(RefreshToken)}," +
                                                        $"{nameof(UserRole)}," +
                                                        $"{nameof(UserRole)}.{nameof(Role)}," +
                                                        $"{nameof(UserRole)}.{nameof(Role)}.{nameof(RoleAction)}," +
                                                        $"{nameof(UserRole)}.{nameof(Role)}.{nameof(RoleAction)}.{nameof(Actions)}"
            );

            ArgumentFalseException.ThrowIfFalse(userData.Status, userData.Message);
            ArgumentThrowCustom.ThrowIfNull<RecordNotFoundException>(userData.Data, "invalid user");
            #endregion
            ResponseUserDetailsWithRoleAndPolicies response = new ResponseUserDetailsWithRoleAndPolicies();
            var user = userData.Data;
            response = user.ToResponseUserDetailsWithActions();
            return response;
        }



        public bool IsUserExist(string email)
        {
           var result=  _uot.user.Any(x=>x.Email==email && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new RecordNotFoundException(result.Message);
            }
        }



        public bool IsUserExistByUserId(string pUserId)
        {
            var result = _uot.user.Any(x => x.Id == pUserId && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new RecordNotFoundException(result.Message);
            }
        }



        public async Task<List<ResponseGetUser>> ListUser(CancellationToken cancellationToken)
        {
            var result = await _uot.user.GetAllAsync(cancellationToken);
            if (result.Status)
            {

                if (result.Data == null)
                {
                    return new List<ResponseGetUser>();

                }
                return result.Data.ToListResponseModel();
            }
            else
            {
                throw new RecordNotFoundException(result.Message);
            }
        }

      

        public async Task<bool> UpdateUser(RequestUpdateUser request, string userId, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var getterResult = await _uot.user.GetByIdAsync(request.UserId, cancellationToken);
            if (getterResult.Status == false)
            {
                throw new RecordNotFoundException("No user with such userId found");
            }
            if (getterResult.Data.UserType == KDefinedRoles.SuperAdmin)
            {
                throw new InvalidOperationException("Can't update SuperAdmin");
            }
            var userWithSameEmail = await _uot.user.GetAsync(cancellationToken, x => x.Email == request.Email && x.Id != request.UserId);
            if (userWithSameEmail.Data.Any())
            {
                throw new InvalidOperationException("The provided email is already assigned to a different user.");
            }

            var user = getterResult.Data;
            user.Name = request.Name;
            user.Email = request.Email;
            user.UpdatedBy = userId;
            user.UpdatedDate = DateTime.UtcNow;

            await _uot.CommitAsync(cancellationToken);

            return true;
        }


        public async Task<int> GetTotalUsers(CancellationToken cancellationToken)
        {
            var result = await _uot.user.GetAllAsync(cancellationToken);

            if (result.Status)
            {
                return result.Data?.Count() ?? 0;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }



    }
}