using BS.CustomExceptions.Common;
using DA;
using DM.DomainModels;

using BS.Services.RoleService.Models.Request;
using BS.Services.UserService.Models.Response;
using BS.Services.UserService.Models.Request;

namespace BS.Services.UserService.Models
{
    public class UserService : IUserService
    {
        IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddUser(RequestAddUser request, string updatedBy, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            string ph = "";
            string ps = "";
            string userId = Guid.NewGuid().ToString();
            User User = request.ToDomainModel( updatedBy, now, ph, ps, userId);

            var result = await _unitOfWork.user.AddAsync(User, userId, cancellationToken);
            if (result.Result)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                return true;
            }
            throw new UnknownException(result.Message);



        }

        
        public async Task<bool> DeleteUser(RequestDeleteUser request, string userId, CancellationToken cancellationToken)
        {
           var result=await _unitOfWork.user.UpdateOnConditionAsync(x => x.Id == request.UserId && x.IsActive,
                x => x.SetProperty(x => x.IsActive, false)
                .SetProperty(x => x.UpdatedBy, userId)
                .SetProperty(x => x.UpdatedDate, DateTime.UtcNow),
                cancellationToken

                );
            if (result.Result)
            {
                return (int)result.Data > 0;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }
     



        public async Task<ResponseGetUser> GetUser(string UserId, CancellationToken cancellationToken)
        {
            var result= await _unitOfWork.user.GetSingleAsync(cancellationToken,x=>x.Id == UserId && x.IsActive);
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

   

        public bool IsUserExist(string email)
        {
           var result=  _unitOfWork.user.Any(x=>x.Email.ToLower()==email.ToLower() && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }
        public bool IsUserExistByUserId(string pUserId)
        {
            var result = _unitOfWork.user.Any(x => x.Id.ToLower() == pUserId.ToLower() && x.IsActive);
            if (result.Status)
            {
                return result.Data;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }

        public async Task<List<ResponseGetUser>> ListUser( CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.user.GetAllAsync(cancellationToken);
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
                throw new UnknownException(result.Message);
            }
        }

      

        public async Task<bool> UpdateUser(RequestUpdateUser request, string userId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.user.UpdateOnConditionAsync(x => x.Id == request.UserId && x.IsActive,
                   x => x.SetProperty(x => x.Name,request.Name)
                   .SetProperty(x=>x.Email,request.Email)
                   
                   .SetProperty(x => x.UpdatedBy, userId)
                   .SetProperty(x => x.UpdatedDate, DateTime.UtcNow),
                   cancellationToken

                   );
            if (result.Result)
            {
                return (int)result.Data > 0;
            }
            else
            {
                throw new UnknownException(result.Message);
            }
        }
    }
}