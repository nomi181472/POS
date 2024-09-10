using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.AuthManagement
{
    public class ChangePassword : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
                            .MapPost($"/{nameof(ChangePassword)}", Handle)
                            .WithSummary("Login a user")
                            .WithRequestValidation<RequestChangePassword>()
                            .Produces(200)
                            .Produces<ResponseChangePassword>();


        public class RequestValidator : AbstractValidator<RequestChangePassword>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.CurrentPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEqual(x => x.CurrentPassword);
                RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
            }
        }

        private static async Task<IResult> Handle(RequestChangePassword request, IAuthService _auth, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await _auth.ChangePassword(request, cancellationToken);
                //  var token = jwt.GenerateToken(new UserPayload() { Id = "1", RoleIds = new[] { "a", "b" }, PolicyName = KPolicyDescriptor.SuperAdminPolicy });
                //result.Token = token;//.Token;
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception e)
            {

                statusCode = HTTPStatusCode500.InternalServerError;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }



        }
    }
}
