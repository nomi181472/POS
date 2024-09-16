using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerManagementService;
using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.CustomerManagementService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Till.Feature.CustomerManagement
{
    public class UpdateCustomer : ICustomerFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateCustomer)}", Handle)
            .WithSummary("Update Cash Details")
            .Produces(200);

        public class RequestValidator : AbstractValidator<RequestUpdateCustomer>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestUpdateCustomer request, ICustomerManagementService customerManagementService, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await customerManagementService.UpdateCustomer(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseUpdateCustomer();
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