using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.OrderService;
using BS.Services.OrderService.Models.Request;
using BS.Services.OrderService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;

namespace Till.Feature.OrderManagement
{
    public class UpdateOrderDetails : IOrderFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateOrderDetails)}", Handle)
            .WithSummary("Update Order Details")
            .Produces(200);

        public class RequestValidator : AbstractValidator<RequestUpdateOrderDetails>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestUpdateOrderDetails request, IOrderDetailsService cash, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await cash.UpdateOrderDetails(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseUpdateOrderDetails();
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