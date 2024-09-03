using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Common.Auth;
using Google.Protobuf.WellKnownTypes;
using Helpers.Auth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWT
{
    public class JwtOptions
    {
        public required string Key { get; init; }
        public required int AccessTokenExpirationInMinutes { get; set; }
        public required int RefreshTokenExpirationInDays { get; set; }
    }

    public class Jwt(IOptions<JwtOptions> options,IConfiguration configuration)
    {
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

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetSubject(payload),
                Expires = DateTime.UtcNow.AddDays(options.Value.RefreshTokenExpirationInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return new AccessAndRefreshTokens
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = tokenHandler.WriteToken(refreshToken)
            };
        }

        private static ClaimsIdentity GetSubject(UserPayload payload)
        {
            return new ClaimsIdentity(new Claim[]
                            {
                    new Claim(ClaimTypes.Email, payload.Email),
                    new Claim(KAuthClaimTypes.UserId,payload.UserId),
                    new Claim(KAuthClaimTypes.UserType,payload.PolicyName),
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