using AttendanceService.Common;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.NotificationManagementService;
using BS.Services.NotificationManagementService.Models.Request;
using FluentValidation;
using Helpers.CustomExceptionThrower;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.NotificationManagement
{
    public class UpdateOnClickNotification : INotificationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateOnClickNotification)}", Handle)
            .WithSummary("Update on Click Notifications")
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestUpdateOnClickNotification>
        {
            INotificationManagementService _service;
            public RequestValidator(INotificationManagementService service)
            {
                _service = service;


            }
        }

        private static async Task<IResult> Handle(RequestUpdateOnClickNotification request, IUserContext userContext, INotificationManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.UpdateOnClickNotification(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentFalseException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
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
