using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
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
    }
}
