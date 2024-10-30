using AttendanceService.Common;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.NotificationManagementService;
using BS.Services.NotificationManagementService.Models.Response;
using Helpers.CustomExceptionThrower;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.NotificationManagement
{
    public class ListNotificationUserWise : INotificationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapGet($"/{nameof(ListNotificationUserWise)}", Handle)
                           .WithSummary("List Notification Details with respect to User")
                           .Produces(200)
                           .Produces(400)
                           .Produces(404)
                           .Produces(500)
                           .Produces<ResponseNotificationLogs>();

        private static async Task<IResult> Handle(string userId, INotificationManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.ListNotificationUserWise(userId, cancellationToken);
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
            catch (ArgumentFalseException e)
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
