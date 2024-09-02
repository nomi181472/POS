using AttendanceService.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CashManagementService;
using BS.Services.CashManagementService.Models.Response;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;

namespace Till.Feature.CashManagement
{
    public class ListCashWithDetails : ICashFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(ListCashWithDetails)}", Handle)
            .WithSummary("sign up a user")
            .Produces(200)
            .Produces<ResponseListCash>();

        //public class RequestValidator : AbstractValidator<RequestAddCash>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(ICashManagementService cash, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await cash.ListCashWithDetails("", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseListCash();
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