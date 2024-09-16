using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CashManagementService.Models.Response;
using BS.Services.CashSessionManagementService;
using BS.Services.CashSessionManagementService.Models.Response;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;

namespace Till.Feature.CashSessionManagement
{
    public class GetCashDetailsByCashSessionId : ICashSessionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetCashDetailsByCashSessionId)}", Handle)
            .WithSummary("List all Cash Details")
            .Produces(200)
            .Produces<ResponseCashSessionDetails>();

        //public class RequestValidator : AbstractValidator<RequestAddCash>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(string cashSessionId, ICashSessionService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.GetCashDetailsByCashSessionId(cashSessionId, "", cancellationToken);
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
