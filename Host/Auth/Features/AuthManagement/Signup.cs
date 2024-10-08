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
using DA.Common.CommonRoles;
using FluentValidation;
using Helpers.Auth.Models;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PaymentGateway.API.Common;

namespace Auth.Features.AuthManagement
{
    public class SignUp : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(SignUp)}", Handle)
            .WithSummary("sign up a user")
            .WithRequestValidation<RequestSignUp>()
            .Produces(200)
            .Produces<ResponseAuthorizedUser>();
        public class RequestValidator : AbstractValidator<RequestSignUp>
        {  
            public RequestValidator()
            {
                RuleFor(n => n.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                    .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Name cannot be only whitespaces.");
                RuleFor(x => x.Email).EmailAddress().Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").NotEmpty();
                RuleFor(m => m.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
                RuleFor(u => u.UserType)
                    .Must(UserType => !string.IsNullOrWhiteSpace(UserType)).WithMessage("UserType cannot be only whitespaces.")
                    .Must(ShouldNotBeSuperAdmin).WithMessage("Invalid UserType");
            }
            private bool ShouldNotBeSuperAdmin(string userType)
            {
                return userType.ToLower() != KDefinedRoles.SuperAdmin.ToLower();
            }
        }

        private static async Task<IResult> Handle(RequestSignUp request, IAuthService _auth, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {

                var result = await _auth.SignUp(request, cancellationToken);
              //var token = jwt.GenerateToken(new UserPayload() { Id = "1", RoleIds = new[] { "a", "b" }, PolicyName = KPolicyDescriptor.CustomPolicy });
             // result.Token = token ;
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch(RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch (InvalidDataException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
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