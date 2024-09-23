using AttendanceService.Common;
using Auth.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.ExternalServices.GrpcClients.Models;
using BS.Services.ActionsService;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.ActionsManagement
{
    public class GetAllAction : IFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                           .MapGet($"/{nameof(GetAllAction)}", Handle)
                           .WithSummary("Add Role Details")
                           .Produces(200)
                           .Produces<ResponseGetAllActions>();

        private static async Task<IResult> Handle(IUserContext userContext, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await actionService.GetAllAction(userContext.Data.UserId, cancellationToken);
                ResponseGetAllActions response = new ResponseGetAllActions();
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
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
