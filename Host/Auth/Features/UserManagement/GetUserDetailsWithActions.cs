using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UserService.Models;
using BS.Services.UserService.Models.Request;
using BS.Services.UserService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.UserManagement
{
    public class GetUserDetailsWithActions : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetUserDetailsWithActions)}/" +"{Id}", Handle)
            .WithSummary("Get user")
            .Produces(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces<bool>();


        private static async Task<IResult> Handle(string Id, IUserService userService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await userService.GetUserDetailsWithActions(Id, cancellationToken);
              
                
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
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
