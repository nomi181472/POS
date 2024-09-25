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
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using FluentValidation;
using Helpers.Auth.Models;
using Logger;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common; 

namespace Auth.Features.AuthManagement
{
    public class Login : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(Login)}", Handle)
            .WithSummary("Login a user")
            .WithRequestValidation<RequestLogin>()
            .Produces(200)
            .Produces(404)
            .Produces(400)
            .Produces(403)
            .Produces(500)
            .Produces<ResponseAuthorizedUser>();


        public class RequestValidator : AbstractValidator<RequestLogin>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestLogin request, IAuthService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await service.Login(request, cancellationToken);
                //  var token = jwt.GenerateToken(new UserPayload() { Id = "1", RoleIds = new[] { "a", "b" }, PolicyName = KPolicyDescriptor.SuperAdminPolicy });
                //result.Token = token;//.Token;
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {

                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (UnknownException e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
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