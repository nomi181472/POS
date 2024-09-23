using BS.CustomExceptions.Common;
using DA;
using DM.DomainModels;

using BS.Services.RoleService.Models.Request;
using BS.Services.UserService.Models.Response;
using BS.Services.UserService.Models.Request;
using Helpers.CustomExceptionThrower;

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
                throw new ArgumentNullException(nameof(request));
            }

            var now = DateTime.UtcNow;
            string ph = "";
            string ps = "";
            string userId = Guid.NewGuid().ToString();
            User User = request.ToDomainModel(updatedBy, now, ph, ps, userId);

            var superAdminRoleExists = await _uot.role.AnyAsync(cancellationToken, r => request.RoleIds.Contains(r.Id) && r.Name == "SuperAdmin");
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
            var getterResult = await _uot.user.GetAsync(cancellationToken, x => x.Id == request.UserId && x.IsActive && x.UserType != "SuperAdmin");
            var userToUpdate = getterResult.Data.FirstOrDefault();
            if (userToUpdate == null)
            {
                throw new RecordNotFoundException("User not Found or is SuperAdmin that can't be deleted");
            }

            userToUpdate.IsActive = false;
            userToUpdate.UpdatedBy = userId;
            userToUpdate.UpdatedDate = DateTime.UtcNow;

            await _uot.CommitAsync(cancellationToken);
            return true;
        }



        public async Task<ResponseGetUser> GetUser(string UserId, CancellationToken cancellationToken)
        {
            if(UserId == null)
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
            if(id == null)
            {
                throw new ArgumentNullException("Id can't be null");
            }
            #region Getting user
            var userData = await _uot.user
                .GetSingleAsync(
                cancellationToken,
                u => u.Id == id,
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
           var result=  _uot.user.Any(x=>x.Email.ToLower()==email.ToLower() && x.IsActive);
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
            var result = _uot.user.Any(x => x.Id.ToLower() == pUserId.ToLower() && x.IsActive);
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
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await _uot.user.UpdateOnConditionAsync(x => x.Id == request.UserId && x.IsActive,
                   x => x.SetProperty(x => x.Name,request.Name)
                   .SetProperty(x=>x.Email,request.Email)
                   .SetProperty(x => x.UpdatedBy, userId)
                   .SetProperty(x => x.UpdatedDate, DateTime.UtcNow),
                   cancellationToken);
            if (result.Result)
            {
                return (int)result.Data > 0;
            }
            else
            {
                throw new InvalidOperationException(result.Message);
            }
        }
    }
}