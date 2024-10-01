using AttendanceService.Common;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.RoleService.Models;
using BS.Services.RoleService.Models.Response;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.RoleManagement
{
    public class ListRolesWithActions : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
           .MapGet($"/{nameof(ListRolesWithActions)}", Handle)
           .WithSummary("List roles")
           .Produces(HTTPStatusCode200.Ok)
           .Produces(HTTPStatusCode400.Forbidden)
           .Produces<ResponseListRolesWithActions>();



        private static async Task<IResult> Handle(IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await roleService.ListRolesWithActions(cancellationToken);
                var response = new ResponseListRolesWithUsers();
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
