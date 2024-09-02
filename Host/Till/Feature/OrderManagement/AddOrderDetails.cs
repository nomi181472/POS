using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.OrderService;
using BS.Services.OrderService.Models.Request;
using BS.Services.OrderService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.OrderManagement
{
    public class AddOrderDetails : IOrderFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddOrderDetails)}", Handle)
            .WithSummary("sign up a user")
            .WithRequestValidation<RequestAddOrderDetails>()
            .Produces(200)
            .Produces<ResponseAddOrderDetails>();

        public class RequestValidator : AbstractValidator<RequestAddOrderDetails>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddOrderDetails request, IOrderDetailsService orderDetails, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await orderDetails.AddOrders(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddOrderDetails();
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
