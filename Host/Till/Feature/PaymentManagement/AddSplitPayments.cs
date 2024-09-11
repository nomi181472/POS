using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PaymentManagementService;
using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.PaymentManagement
{
    public class AddSplitPayments : IPaymentManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddSplitPayments)}", Handle)
            .WithSummary("Calculate Surcharge and Discount Details")
            .WithRequestValidation<RequestAddSplitPayments>()
            .Produces(200)
            .Produces<ResponseAddSplitPayments>();

        public class RequestValidator : AbstractValidator<RequestAddSplitPayments>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddSplitPayments request, IPaymentManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.AddSplitPayments(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddSplitPayments();
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
