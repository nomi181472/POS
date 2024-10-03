using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CustomerFeedbackService;
using BS.Services.CustomerFeedbackService.Models.Request;
using BS.Services.CustomerFeedbackService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Common;
using Till.Extensions.RouteHandler;
using Till.Middlewares;

namespace Till.Feature.CustomerFeedbackManagement
{
    public class AddCustomerFeedback : ICustomerFeedbackFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddCustomerFeedback)}", Handle)
            .WithSummary("Add Cash Details")
            .WithRequestValidation<RequestAddCustomerFeedback>()
            .Produces(200)
            .Produces<ResponseAddCustomerFeedback>();

        public class RequestValidator : AbstractValidator<RequestAddCustomerFeedback>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddCustomerFeedback request, IUserContext userContext, ICustomerFeedbackService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {

            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.AddCustomerFeedback(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddCustomerFeedback();
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
