using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UserService.Models;
using BS.Services.UserService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.UserManagement
{
    public class ListUsers : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListUsers)}", Handle)
            .WithSummary("List all users")
           
            .Produces(200)
            .Produces<List<ResponseGetUser>>();


        private static async Task<IResult> Handle(IUserService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.ListUser( cancellationToken);
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
