using Auth.Common.Auth;
using AuthJWT;
using Microsoft.Extensions.Primitives;
using SessionManager;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RedisSessionManager _redisSessionManager;
        private readonly Jwt _jwt;

        public SessionMiddleware(RequestDelegate next, RedisSessionManager redisSessionManager, Jwt jwt)
        {
            _next = next;
            _redisSessionManager = redisSessionManager;
            _jwt = jwt;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            #region Excluded Routes
            var excludedPaths = new List<string>
            {
                "/",
                "/swagger/index.html",
                "/Auth/IAuthFeature/SignUp",
                "/Auth/IAuthFeature/Login",
                "/Auth/IAuthFeature/RefreshToken",
                "/Auth/IAuthFeature/ForgetPassword",
                "/Auth/IAuthFeature/ChangePassword"
            };
            if (excludedPaths.Contains(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }
            #endregion Excluded Routes

            #region Fetch Token from Header
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues tokenValues))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }
            var token = tokenValues.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Bearer token is missing.");
                return;
            }
            #endregion Fetch Token from Header

            #region Read Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            #endregion Read Token

            #region Fetch UserId
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == KAuthClaimTypes.UserId);
            if (userIdClaim == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
                return;
            }
            var userId = userIdClaim.Value;
            #endregion Fetch UserId

            #region Fetch from Redis
            var storedToken = await _redisSessionManager.GetTokenAsync(userId);
            if (storedToken == null || storedToken != token)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token.");
                return;
            }
            #endregion Fetch from Redis

            await _next(context);
        }
    }
}