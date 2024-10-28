using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ActionsService;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.ActionsManagement
{
    public class AppendActionTag : IActionsFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AppendActionTag)}", Handle)
            .WithSummary("Add a new tag to the list of tags in action")
            .WithRequestValidation<RequestAppendActionTag>()
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseAppendActionTag>();

        public class RequestValidator : AbstractValidator<RequestAppendActionTag>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAppendActionTag request, IUserContext userContext, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await actionService.AppendActionTag(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAppendActionTag();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
