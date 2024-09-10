using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService
{
    public interface IAuthService
    {
        Task<ResponseAuthorizedUser> SignUp(RequestSignUp request, CancellationToken token);
        Task<ResponseAuthorizedUser> Login(RequestLogin request, CancellationToken token);
        Task<ResponseForgetPassword> ForgetPassword(RequestForgetPassword request, CancellationToken token);
        Task<ResponseChangePassword> ChangePassword(RequestChangePassword request, CancellationToken token);
    }
}
