using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ActionsService;
using BS.Services.ActionsService.Models.Request;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.ActionsManagement
{
    public class RemoveActionTag : IActionsFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapPatch($"/{nameof(RemoveActionTag)}", Handle)
                           .WithSummary("remove tags from actions")
                           .WithRequestValidation<RequestRemoveActionTag>()
                           .Produces(200)
                           .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestRemoveActionTag>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestRemoveActionTag request, IUserContext userContext, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await actionService.RemoveActionTag(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (InvalidOperationException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
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
