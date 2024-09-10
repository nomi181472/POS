
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ActionExposedGRPC;
using AttendanceService.Common;
using Auth.Common;
using Auth.Common.Constant;
using AuthJWT;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.ExternalServices;
using BS.ExternalServices.GrpcClients;
using EndpointGRPC.Exposed;
using FluentValidation;
using Helpers;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common;

namespace Auth.Features.RouteManagement
{
    public class GetAllEndpoints : IActionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAllEndpoints)}", Handle)
            .WithSummary("TestEndpoint");
        

        private static async Task<IResult> Handle(IActionControllerService action, ICustomLogger _logger, Jwt jwt, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
               
                string ApiBaseName = KConstant.ApiName;
                Assembly assembly = Assembly.GetExecutingAssembly();
                var iFeature = typeof(IFeature);
                var result=await action.GetAllActionsOfAllApis(ApiBaseName, assembly, iFeature, cancellationToken);
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