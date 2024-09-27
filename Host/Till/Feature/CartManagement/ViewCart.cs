using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.SaleProcessingService;
using BS.Services.SaleProcessingService.Models.Request;
using BS.Services.SaleProcessingService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using System.Reflection.Metadata;
using Till.Common;
using Till.Extensions.RouteHandler;

namespace Till.Feature.CartManagement
{
    public class ViewCart : ICartFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ViewCart)}", Handle)
            .WithSummary("View cart")
            .WithRequestValidation<ViewCartRequest>()
            .Produces(200)
            .Produces(201)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<ViewCartResponse>();

        public class RequestValidator : AbstractValidator<ViewCartRequest>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(string? CartId, ISaleProcessingService _saleProcessing, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                string userId = "";//fetch from header
                var result = await _saleProcessing.ViewCart(CartId, userId, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException ex)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, false);
            }
            catch (ArgumentException ex)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = ex.Message;
                _logger.LogError(ex.Message, ex);
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
