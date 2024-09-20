using AttendanceService.Common;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.RoleService.Models;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.RoleManagement
{
    public class GetPoliciesByRoleId : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetPoliciesByRoleId)}/" + "{Id}", Handle)
            .WithSummary("Get role")
            .Produces(200)
            .Produces<bool>();


        private static async Task<IResult> Handle(string Id, IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await roleService.GetPoliciesByRoleId(Id, cancellationToken);

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
