using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerManagementService;
using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.CustomerManagement
{
    public class AddCustomer : ICustomerFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddCustomer)}", Handle)
            .WithSummary("Add Customer Details")
            .WithRequestValidation<RequestAddCustomer>()
            .Produces(200)
            .Produces<ResponseAddCustomer>();

        public class RequestValidator : AbstractValidator<RequestAddCustomer>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddCustomer request, ICustomerManagementService customer, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await customer.AddCustomer(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddCustomer();
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
