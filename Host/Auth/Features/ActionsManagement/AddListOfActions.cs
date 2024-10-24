using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using BS.Services.ActionsService;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using AttendanceService.Common;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;

namespace Auth.Features.ActionsManagement
{
    public class AddListOfActions : IActionsFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddListOfActions)}", Handle)
            .WithSummary("Add List of Actions")
            .Produces(200)
            .Produces<bool>();

        private static async Task<IResult> Handle(RequestAddListOfActions request, IUserContext userContext, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await actionService.AddListOfActions(request, userContext.Data.UserId, cancellationToken);
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
            catch (ArgumentException e)
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
