using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AttendanceService.Common;
using Auth.Common;
using Auth.Common.Constant;
using Auth.Extensions.RouteHandler;
using AuthJWT;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using EndpointGRPC.Exposed;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common;

namespace Auth.Features.RouteManagement
{
    public class GetAllEndpoints : IRouteFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetAllEndpoints)}", Handle)
            .WithSummary("TestEndpoint");
        

        private static async Task<IResult> Handle( ICustomLogger _logger, Jwt jwt, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                string ApiBaseName = KConstant.ApiName;
                Assembly assembly = Assembly.GetExecutingAssembly();
                var iFeature = typeof(IFeature);
                Dictionary<string, List<string>> apiEndpoints = ExposedAllEndpoints.GetAllActions(ApiBaseName, assembly, iFeature);

                return ApiResponseHelper.Convert(true, true, message, statusCode, apiEndpoints);
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