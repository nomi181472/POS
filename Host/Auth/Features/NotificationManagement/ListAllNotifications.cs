using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.NotificationManagementService.Models.Request;
using BS.Services.NotificationManagementService;
using DM.DomainModels;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using BS.Services.NotificationManagementService.Models.Response;

namespace Auth.Features.NotificationManagement
{
    public class ListAllNotifications : INotificationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllNotifications)}", Handle)
            .WithSummary("List all Notification Details")
            .Produces(200)
            .Produces(404)
            .Produces(500)
            .Produces<ResponseNotificationLogs>();

        private static async Task<IResult> Handle(INotificationManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.ListAll(cancellationToken);
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
