using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AuthService;
using BS.Services.AuthService.Models.Request;
using BS.Services.AuthService.Models.Response;
using BS.Services.RoleService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.AuthManagement
{
    public class ForgetPassword : IAuthFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(ForgetPassword)}", Handle)
            .WithSummary("Send it to email")
            .WithRequestValidation<RequestForgetPassword>()
            .Produces(200)
            .Produces<ResponseForgetPassword>();


        public class RequestValidator : AbstractValidator<RequestForgetPassword>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestForgetPassword request, IAuthService _auth, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {

                var result = await _auth.ForgetPassword(request, cancellationToken);
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
