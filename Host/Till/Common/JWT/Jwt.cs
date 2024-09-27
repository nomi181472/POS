﻿using Helpers.Auth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Till.Common.Auth;

namespace Till.Common.JWT
{
    public class JwtOptions
    {
        public required string Key { get; init; }
        public required int AccessTokenExpirationInMinutes { get; set; }
        public required int RefreshTokenExpirationInDays { get; set; }
    }

    public class Jwt(IOptions<JwtOptions> options, IConfiguration configuration)
    {
        public UserPayload GetUserPayloadFromClaims(ClaimsPrincipal user)
        {
            var userPayload = new UserPayload
            {
                Email = user.FindFirst(KAuthClaimTypes.Email)?.Value,
                UserId = user.FindFirst(KAuthClaimTypes.UserId)?.Value,
                UserType = user.FindFirst(KAuthClaimTypes.UserType)?.Value,
                RoleIds = user.FindFirst(KAuthClaimTypes.Resources)?.Value
            };

            return userPayload;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public AccessAndRefreshTokens GenerateToken(UserPayload payload)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(options.Value.Key);

            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetSubject(payload),
                Expires = DateTime.UtcNow.AddMinutes(options.Value.AccessTokenExpirationInMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var refreshToken = GenerateRefreshToken();

            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);


            return new AccessAndRefreshTokens
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        }

        private static ClaimsIdentity GetSubject(UserPayload payload)
        {
            return new ClaimsIdentity(new Claim[]
                            {
                    new Claim(KAuthClaimTypes.Email, payload.Email),
                    new Claim(KAuthClaimTypes.UserId,payload.UserId),
                    new Claim(KAuthClaimTypes.UserType,payload.UserType),
                    new Claim(KAuthClaimTypes.Resources,payload.RoleIds)
                            });
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(options.Value.Key);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public static SymmetricSecurityKey SecurityKey(string key) => new(Encoding.ASCII.GetBytes(key));
    }
}
