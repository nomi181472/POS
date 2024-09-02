using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AttendanceService.Common;
using Till.Common;
using Till.Extensions.RouteHandler;
//using BS.CustomExceptions.CustomExceptionMessage;
//using BS.Services.AuthService;
//using BS.Services.AuthService.Models.Request;
//using BS.Services.AuthService.Models.Response;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashManagementService.Models.Response;
using BS.Services.CashManagementService;
using BS.CustomExceptions.CustomExceptionMessage;

namespace Till.Features.CashManagement
{
    public class AddCash : IFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddCash)}", Handle)
            .WithSummary("sign up a user")
            .WithRequestValidation<RequestAddCash>()
            .Produces(200)
            .Produces<ResponseAddCash>();

        public class RequestValidator : AbstractValidator<RequestAddCash>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddCash request, ICashManagementService cash, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await cash.AddCash(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddCash();
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