using BS.CustomExceptions.Common;
using BS.Delegate;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using BS.Services.UserService.Models;
using DA;
using DM.DomainModels;
using Helpers.Auth.Models;
using Helpers.StringsExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Helpers.Strings;
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


            #region Getting user
            var userData = await _uot.user
                .GetSingleAsync(
                token, 
                u => u.Email.ToLower() == request.Email.ToLower(),
                $"{nameof(Credential)}," +
                $"{nameof(UserRole)}," +
                $"{nameof(RefreshToken)}," +
                $"{nameof(UserRole)}.{nameof(Role)}," +
                $"{nameof(UserRole)}.{nameof(Role)}.{nameof(RoleAction)}," +
                $"{nameof(UserRole)}.{nameof(Role)}.{nameof(RoleAction)}.{nameof(Actions)}" 
                );
            #endregion

            if (userData.Status)
            {
                var user = userData.Data;
                if (user == null)
                {
                    throw new RecordNotFoundException("User not found.");
                }

                var credential = user.Credential!;
                var roles = user.UserRole;

                var pwd = request.Password;
                var hash = credential.PasswordHash;
                var salt = credential.PasswordSalt;
                

                if (PasswordHelper.VerifyPassword(pwd, hash,salt) == false)
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }

                var tokens = _generateToken(new UserPayload()
                {
                    UserId = user.Id,
                    UserType = user.UserType,
                    RoleIds = string.Join(KConstantToken.Separator, roles.Select(x => x.ToResponse()).SelectMany(x => x.Actions).Select(x => x.ActionName.ToLower().ToShortenUrl())),
                    Email = user.Email
                });

                response.UserId = user.Id;
                response.RoleAndActions = roles.Select(x=>x.ToResponse());
                response.Token = tokens.AccessToken;
                response.RefreshToken = user.RefreshToken!.Token;
                response.UserType = user.UserType;

                response.Name = user.Name;
                response.Email = user.Email;

                return response;
            }
            else
            {
                throw new UnknownException(userData.Message);
            }
            
        }



        public async Task<ResponseAuthorizedUser> SignUp(RequestSignUp request, CancellationToken token)
        {
            ResponseAuthorizedUser response = new ResponseAuthorizedUser();
           
            string userId = Guid.NewGuid().ToString();
            
            DateTime now = DateTime.Now;
            List<UserRole> userRoles = new List<UserRole>();
            if (request.RoleIds != null && request.RoleIds.Count > 0)
            {
                var existingRoleIds = await _uot.role.GetAsync(token, r => request.RoleIds.Contains(r.Id));
                var validRoleIds = existingRoleIds.Data.Select(r => r.Id).ToHashSet();
                var invalidRoleIds = request.RoleIds.Except(validRoleIds).ToList();

                if (invalidRoleIds.Any())
                {
                    throw new RecordNotFoundException($"The following role IDs do not exist: {string.Join(", ", invalidRoleIds)}");
                }

                List<UserRole> userRolesWithRoleId = request.RoleIds.Select(roleId => new UserRole(userId, roleId, Guid.NewGuid().ToString(), userId, now)).ToList();
                userRoles = userRolesWithRoleId;
            }

            var existingUserResult = await _uot.user.GetAsync(token, u => u.Email == request.Email);
            var existingUser = existingUserResult.Data.FirstOrDefault();

            if (existingUser != null)
            {
                throw new InvalidDataException("Email already registered");
            }

            if (request.ConfirmPassword != request.Password)
            {
                throw new InvalidDataException("Passwords don't match");
            }

            var hAndS =PasswordHelper.HashPassword(request.Password);
            string passwordHash = hAndS.hash;
            string passwordSalt = hAndS.salt;

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
                UserType = request.UserType,
                RoleIds = string.Join(";", request.RoleIds),
                Email = request.Email,
            });

            response.UserType = user.UserType;
            response.UserId = user.Id;
            response.RoleAndActions = new List<ResponsePolicyByRoleId>();
            response.RefreshToken = tokens.RefreshToken;
            response.Token = tokens.AccessToken;

            return response;
        }



        public async Task<ResponseForgetPassword> ForgetPassword(RequestForgetPassword request, CancellationToken token)
        {
            var userResult = await _uot.user.GetAsync(token, u => u.Email == request.Email);
            var user = userResult.Data.FirstOrDefault();

            if(user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }

            string resetToken = Guid.NewGuid().ToString();

            var credentialResult = await _uot.creadential.GetAsync(token, c => c.UserId == user.Id);
            var credential = credentialResult.Data.FirstOrDefault();
            credential.PasswordHash = resetToken; // currently storing it in PasswordHash since it would be reset already
            var updateResult = _uot.creadential.Update(credential, credential.Id);
            // TODO: SEND PASSWORD TO EMAIL
            if (updateResult.Result) // ADD CONDITION TO CHECK IF SENT TO EMAIL
            {
                await _uot.CommitAsync(token);
                ResponseForgetPassword response = new ResponseForgetPassword()
                {
                    Message = "your password has been sent to email: " + user.Email
                };
                return response;
            }
            else
            {
                throw new InvalidOperationException("Failed to send Reset Token due to: " + updateResult.Message);
            }
        }



        public async Task<ResponseChangePassword> ChangePassword(RequestChangePassword request, CancellationToken token)
        {
            var userResult = await _uot.user.GetAsync(token, u => u.Email == request.Email);
            var user = userResult.Data.FirstOrDefault();

            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }

            var credentialResult = await _uot.creadential.GetAsync(token, c => c.UserId == user.Id);
            var credential = credentialResult.Data.FirstOrDefault();

            if (credential == null)
            {
                throw new UnauthorizedAccessException("User credentials not found.");
            }

            if (!PasswordHelper.VerifyPassword(request.CurrentPassword, credential.PasswordHash, credential.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Invalid current password.");
            }

            var newHashAndSalt = PasswordHelper.HashPassword(request.NewPassword);
            string newHash = newHashAndSalt.hash;
            string newSalt = newHashAndSalt.salt;

            credential.PasswordHash = newHash;
            credential.PasswordSalt = newSalt;

            var updateResult = _uot.creadential.Update(credential, credential.Id);
            if (updateResult.Result)
            {
                await _uot.CommitAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Failed to change password due to: " + updateResult.Message);
            }

            ResponseChangePassword response = new ResponseChangePassword()
            {
                Message = "Your password has been successfully changed."
            };

            return response;
        }
        

        public async Task<AccessAndRefreshTokens> UpdateAndGetRefreshToken(AccessAndRefreshTokens request,string claimUserId,string userId, string actions, CancellationToken token)
        {
            var userData=await _uot.user.GetSingleAsync(token, x => x.Id == claimUserId, $"{nameof(RefreshToken)}");

            if (userData.Status)
            {
                if (userData.Data == null)
                {
                    throw new UnauthorizedAccessException("invalid refresh token");
                }
                var user = userData.Data;
                var refreshToken = user.RefreshToken!;
                if (!refreshToken.IsTokenMatch(request.RefreshToken))
                {
                    throw new UnauthorizedAccessException("invalid refresh token");
                }
               

                var tokens = _generateToken(new UserPayload()
                {
                    UserId = user.Id,
                    UserType = user.UserType,
                    RoleIds = actions,
                    Email = user.Email
                });
                request.RefreshToken = tokens.RefreshToken;
                request.AccessToken = tokens.AccessToken;

                refreshToken.UpdateRefreshToken(DateTime.UtcNow.AddMinutes(KTokenValidity.RefreshTokenInMin), request.RefreshToken, userId) ;
                _uot.refereshToken.Update(refreshToken, userId);
                _uot.Commit();
                
                return request;

            }
            throw new UnknownException(userData.Message);

            
        }
    }
}