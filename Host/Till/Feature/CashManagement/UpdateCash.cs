using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CashManagementService;
using BS.Services.CashManagementService.Models.Request;
using BS.Services.CashManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;

namespace Till.Feature.CashManagement
{
    public class UpdateCash : ICashFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateCash)}", Handle)
            .WithSummary("Update Cash Details")
            .Produces(200);

        public class RequestValidator : AbstractValidator<RequestUpdateCash>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestUpdateCash request, ICashManagementService cash, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await cash.UpdateCash(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseUpdateCash();
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