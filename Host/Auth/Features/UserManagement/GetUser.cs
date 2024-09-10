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
    public class GetUser : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(GetUser)}/" +"{Id}", Handle)
            .WithSummary("Get user")
            .Produces(200)
            .Produces<bool>();


        private static async Task<IResult> Handle(string Id, IUserService userService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await userService.GetUser(Id, cancellationToken);
              
                
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
