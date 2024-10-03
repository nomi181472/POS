using Helpers.Auth;
using Helpers.Auth.Models;

namespace Till.Middlewares
{
    public interface IUserContext { UserPayload Data { get; set; } }

    public class UserContext : IUserContext { public UserPayload Data { get; set; } }

    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            userContext.Data = HTTPContextUserRetriever.GetUserPayloadFromClaims(context.User);
            await _next(context);
        }
    }

}
