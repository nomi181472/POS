
using BS.Services.RoleService.Models.Request;
using BS.Services.UserService.Models.Request;
using BS.Services.UserService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.UserService.Models
{
    public interface IUserService
    {
      
      
      
        bool IsUserExist(string UserName);
        bool IsUserExistByUserId(string UserId);
        Task<bool> AddUser(RequestAddUser request, string userId, CancellationToken cancellationToken);
        Task<bool> DeleteUser(RequestDeleteUser request, string userId, CancellationToken cancellationToken);
        Task<bool> UpdateUser(RequestUpdateUser request, string userId, CancellationToken cancellationToken);
        Task<ResponseGetUser> GetUser(string UserId, CancellationToken cancellationToken);
        Task<List<ResponseGetUser>> ListUser( CancellationToken cancellationToken);
    }
}
