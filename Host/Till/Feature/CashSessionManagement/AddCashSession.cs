using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CashManagementService.Models.Response;
using BS.Services.CashSessionManagementService;
using BS.Services.CashSessionManagementService.Models.Request;
using BS.Services.CashSessionManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.CashSessionManagement
{
    public class AddCashSession : ICashSessionFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddCashSession)}", Handle)
            .WithSummary("Add Cash Details")
            .WithRequestValidation<RequestAddCashSession>()
            .Produces(200)
            .Produces<ResponseAddCashSession>();

        public class RequestValidator : AbstractValidator<RequestAddCashSession>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddCashSession request, ICashSessionService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.AddCashSession(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddCashSession();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentNullException e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
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
