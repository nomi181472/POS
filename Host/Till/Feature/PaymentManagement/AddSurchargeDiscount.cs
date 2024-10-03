using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PaymentManagementService;
using BS.Services.PaymentManagementService.Models.Request;
using BS.Services.PaymentManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Extensions.RouteHandler;
using Till.Middlewares;

namespace Till.Feature.PaymentManagement
{
    public class AddSurchargeDiscount : IPaymentManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddSurchargeDiscount)}", Handle)
            .WithSummary("Calculate Surcharge and Discount Details")
            .WithRequestValidation<RequestAddSurchargeDiscount>()
            .Produces(200)
            .Produces<ResponseAddSurchargeDiscount>();

        public class RequestValidator : AbstractValidator<RequestAddSurchargeDiscount>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddSurchargeDiscount request, IUserContext userContext, IPaymentManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = true;// await service.AddSurchargeDiscount(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddSurchargeDiscount();
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
