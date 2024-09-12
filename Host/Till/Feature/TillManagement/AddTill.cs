using System.Reflection.Metadata;
using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.CashManagementService.Models.Response;
using BS.Services.TillManagementService;
using BS.Services.TillManagementService.Request;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Till.Extensions.RouteHandler;
using Till.Feature.CashManagement;
using Till.Features.CashManagement;

namespace Till.Feature.TillManagement
{
    public class AddTill : ITillFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddTill)}", Handle)
            .WithSummary("add a new till")
            .WithRequestValidation<RequestAddTill>()
            .Produces(200)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddTill>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddTill request, ITillManagementService till, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var response = await till.AddTill(request, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, response);
            }
            catch (Exception e)
            {

                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
        }
    }
}
