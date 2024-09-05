using AttendanceService.Common;
using Auth.Common.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using PaymentGateway.API.Common;
using System.Security.Claims;

namespace Auth.Common.Auth
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            // Optionally, check the request path

            var httpContext = context.Resource as HttpContext;
            if (httpContext != null)
            {
                var path = httpContext.Request.Path;
                // Optionally, check the request path
                if (IsUserValidated(context))
                {

                    context.Succeed(requirement);
                }
                else
                {
                    //TODO:loging
                    int statusCode = HTTPStatusCode400.Forbidden;
                    _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;
                    _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                    await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(ApiResponseHelper.Convert(true, false, $"User is not authorizeed to access this resource: {path}", statusCode, null));
                    await _httpContextAccessor.HttpContext.Response.CompleteAsync();

                    context.Fail();

                }
            }
            else
            {
                //TODO:loging
                int statusCode = HTTPStatusCode500.ServiceUnavailable;
                _httpContextAccessor.HttpContext.Response.StatusCode = statusCode;
                _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                await _httpContextAccessor.HttpContext.Response.WriteAsJsonAsync(ApiResponseHelper.Convert(true, false, "", statusCode, null));
                await _httpContextAccessor.HttpContext.Response.CompleteAsync();

                context.Fail();
            }

            
          
        }

        private static bool IsUserValidated(AuthorizationHandlerContext context)
        {
            string userId = "";
            return context.User.HasClaim(c => c.Value == "sad");
        }
    }
}
