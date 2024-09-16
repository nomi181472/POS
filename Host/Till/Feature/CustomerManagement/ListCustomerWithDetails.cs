using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerManagementService;
using BS.Services.CustomerManagementService.Models.Response;
using Logger;
using PaymentGateway.API.Common;

namespace Till.Feature.CustomerManagement
{
    public class ListCustomerWithDetails : ICustomerFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListCustomerWithDetails)}", Handle)
            .WithSummary("List all Customer Details")
            .Produces(200)
            .Produces<ResponseListCustomerWithDetails>();

        //public class RequestValidator : AbstractValidator<RequestAddCash>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(ICustomerManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.ListCustomerWithDetails("", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseListCustomerWithDetails();
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