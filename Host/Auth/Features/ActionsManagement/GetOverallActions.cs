using AttendanceService.Common;
using Auth.Common;
using Auth.Common.Constant;
using Auth.Features.ActionsManagement;
using AuthJWT;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.ExternalServices.GrpcClients;
using Logger;
using PaymentGateway.API.Common;
using System.Reflection;

public class GetOverallActions : IActionsFeature
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(GetOverallActions)}", Handle)
        .WithSummary("TestEndpoint")
        .Produces(200)
        .Produces(400);

    private static async Task<IResult> Handle(IActionControllerService action, ICustomLogger _logger, Jwt jwt, CancellationToken cancellationToken)
    {
        int statusCode = HTTPStatusCode200.Ok;
        string message = "Success";
        try
        {
            var result = await action.GetAllActionsOfAllApis(GeneratedApiRoutes.GetOverallActionsApiRoutes.Routes);
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
