using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerManagementService;
using BS.Services.CustomerManagementService.Models.Response;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;

namespace Till.Feature.CustomerManagement
{
    public class GetCustomerHistoryById : ICustomerFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetCustomerHistoryById)}", Handle)
            .WithSummary("List all Cash Details")
            .Produces(200)
            .Produces<ResponseGetCustomerHistoryById>();

        //public class RequestValidator : AbstractValidator<RequestAddCash>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(string customerId, ICustomerManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.GetCustomerHistoryById(customerId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
            {

                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch (RecordNotFoundException e)
            {

                statusCode = HTTPStatusCode400.NotFound;
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
