using MediatR;
using UserActivity;

namespace Hub.Middlewares
{
    public class UserActivityLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMediator _mediator;
        //private readonly IUserActivity _userActivity;

        public UserActivityLoggingMiddleware(RequestDelegate next, IMediator mediator)
        {
            _next = next;
            _mediator = mediator;
            //_userActivity = userActivity;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var userId = context.User?.Identity?.Name ?? "anonymous";
            var activity = $"{context.Request.Method} {context.Request.Path}";

            var command = new UserActivityLogCommand(userId, activity);
            await _mediator.Send(command);

            await _next(context);

            //string userId = context.User?.Identity?.Name ?? "Anonymous";

            //// Log request activity before processing
            //string activity = $"User {userId} accessed endpoint: {context.Request.Path}";
            //await _userActivity.LogActivityAsync(userId, activity);

            //// Call the next middleware in the pipeline
            //await _next(context);
        }
    }
}
