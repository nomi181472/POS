using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AttendanceService.Common;
using Auth.Common;
using Auth.Common.Auth;
using Auth.Extensions.RouteHandler;
using AuthJWT;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.AuthService.Models.Request;
using FluentValidation;
using Helpers.Auth.Models;
using Logger;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common;

namespace Auth.Features.AuthManagement
{
    public class RefreshToken : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(RefreshToken)}", Handle)
            .WithSummary("refreshToken a user");
            


       

        private static async Task<IResult> Handle(AccessAndRefreshTokens request,Jwt jwt, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var principal = jwt.GetPrincipalFromExpiredToken(request.RefreshToken);
                if (principal == null)
                {
                    statusCode = HTTPStatusCode400.Unauthorized; 
                    return ApiResponseHelper.Convert(true, false, message, statusCode, null);
                }

                
                var newTokens = jwt.GenerateToken(jwt.GetUserPayloadFromClaims(principal));
                
                return ApiResponseHelper.Convert(true, true, message, statusCode, newTokens);

               
               
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