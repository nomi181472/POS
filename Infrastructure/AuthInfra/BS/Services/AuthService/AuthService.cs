﻿using BS.CustomExceptions.Common;
using BS.Delegate;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
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

            response.UserId = "UID-1";
            response.RoleIds = ["RID-1", "RID-2"];
            response.Token = "accessToken";
            response.RefreshToken = "refreshToken";
            response.UserType = "Admin";
            response.Name = "Bilal";
            response.Email = request.Email;

            return response;

            var userResult = await _uot.user.GetAsync(token, u => u.Email == request.Email);
            var user = userResult.Data.FirstOrDefault();

            if (user == null)
            {
                throw new RecordNotFoundException("User not found.");
            }

            var credentialResult = await _uot.creadential.GetAsync(token, c => c.UserId == user.Id);
            var credential = credentialResult.Data.FirstOrDefault();

            var pwd = request.Password;
            var hash = credential.PasswordHash;
            var salt = credential.PasswordSalt;
            if(hash == null || salt == null)
            {
                throw new ArgumentNullException("Credentials are being passed null");
            }
            if (PasswordHelper.VerifyPassword(pwd, hash, salt) == false)
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }

            var roleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var userRolesResult = await _uot.role.GetAsync(token, r => roleIds.Contains(r.Id));
            var userRoles = userRolesResult.Data.ToList();

            var roleActionsResult = await _uot.roleAction.GetAsync(token, ra => roleIds.Contains(ra.RoleId));
            var roleActions = roleActionsResult.Data.ToList();

            var policyIds = roleActions.Select(ra => ra.ActionId).Distinct().ToList();

            var tokens = _generateToken(new UserPayload()
            {
                UserId = user.Id,
                PolicyName = user.UserType,
                RoleIds = string.Join(";", userRoles.Select(r => r.Id)),
                Email = user.Email
            });

            response.UserId = user.Id;
            response.RoleIds = userRoles.Select(r => r.Id).ToArray();
            response.Token = tokens.AccessToken;
            response.RefreshToken = tokens.RefreshToken;
            response.UserType = user.UserType;

            response.Name = user.Name;
            response.Email = user.Email;

            return response;
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

            var hAndS = request.Password.CreateHashAndSalt();
            string passwordHash = hAndS.Hash;
            string passwordSalt = hAndS.Salt;

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

            var newHashAndSalt = request.NewPassword.CreateHashAndSalt();
            string newHash = newHashAndSalt.Hash;
            string newSalt = newHashAndSalt.Salt;

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


    }
}