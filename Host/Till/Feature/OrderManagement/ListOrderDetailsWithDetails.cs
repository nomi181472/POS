using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.OrderService;
using BS.Services.OrderService.Models.Response;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;
using Till.Feature.CashManagement;

namespace Till.Feature.OrderManagement
{
    public class ListOrderDetailsWithDetails : IOrderFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListOrderDetailsWithDetails)}", Handle)
            .WithSummary("List All Order Details")
            .Produces(200)
            .Produces<ResponseListOrderDetails>();

        //public class RequestValidator : AbstractValidator<RequestAddCash>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(IOrderDetailsService orders, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await orders.ListOrderDetailsWithDetails("", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseListOrderDetails();
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