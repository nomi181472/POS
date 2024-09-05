using BS.Delegate;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using DA;
using DM.DomainModels;
using Helpers.Auth.Models;
using Helpers.StringsExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Services.AuthService
{
    internal class AuthService : IAuthService

    {
        private readonly Func<UserPayload, AccessAndRefreshTokens> _generateToken;
        readonly IUnitOfWork _uot;
        public AuthService(IUnitOfWork unit, Func<UserPayload, AccessAndRefreshTokens> generateToken) {
            _uot = unit;
            _generateToken = generateToken;
        }    
        public async Task<ResponseAuthorizedUser> Login(RequestLogin request, CancellationToken token)
        {
            ResponseAuthorizedUser response = new ResponseAuthorizedUser();
            //TODO: ge user email and check exist or not
            //
            //compare password and login the user
            //when fetching user, must fetch its roles then policiess and provide in return type where roleIds object is and make
            //AccessAndReferesh tokens
            
            return new ResponseAuthorizedUser() { UserId = Guid.NewGuid().ToString() };
        }

        public async Task<ResponseAuthorizedUser> SignUp(RequestSignUp request, CancellationToken token)
        {
            ResponseAuthorizedUser response = new ResponseAuthorizedUser();
           
            string userId = Guid.NewGuid().ToString();
            


            DateTime now = DateTime.Now;
            List<UserRole> userRoles = new List<UserRole>();
            if (request.RoleIds.Count > 0)
            {
                userRoles = request.RoleIds.Select(roleId => new UserRole(userId,roleId, Guid.NewGuid().ToString(), userId, now)).ToList();
            }
          
            var hAndS = request.Password.CreateHashAndSalt();
            string passwordHash = hAndS.Hash;
            string passwordSalt=hAndS.Salt;



            Credential credential = new Credential(Guid.NewGuid().ToString(), userId, now, passwordSalt, passwordHash, userId);

            User user = new User(userId, userId, now, request.Name, request.Email, "", credential, userRoles);

            var result = _uot.user.Add(user, userId);
            if (result.Result)
            {
                _uot.Commit();
            }

            var tokens = _generateToken(new UserPayload()
            {
                UserId = userId,
                PolicyName = request.UserType,
                RoleIds = string.Join(";", request.RoleIds),
                Email = request.Email,
            });

            response.UserType = user.UserType;
            response.UserId = user.Id;
            response.RoleIds = request.RoleIds.ToArray();
            response.RefreshToken = tokens.RefreshToken;
            response.Token = tokens.AccessToken;



            return response;
        }

        
    }
}