using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.SaleProcessingService;
using BS.Services.SaleProcessingService.Models.Request;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using System.Reflection.Metadata;
using Till.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.CartManagement
{
    public class RemoveCart : ICartFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPatch($"/{nameof(RemoveCart)}", Handle)
            .WithSummary("Remove cart")
            .WithRequestValidation<RemoveCartRequest>()
            .Produces(200)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RemoveCartRequest>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RemoveCartRequest request, ISaleProcessingService _saleProcessing, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                string userId = "";//fetch from header
                var result = await _saleProcessing.RemoveCart(request, userId, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException ex)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
            catch (ArgumentNullException ex)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
            catch (Exception ex)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
        }
    }
}
