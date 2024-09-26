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
using Till.Feature.CartManagement;

namespace Till.Feature.SaleProcessing
{
    public class CreateOrder:ISaleFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(CreateOrder)}", Handle)
            .WithSummary("Create Order")
            .WithRequestValidation<CreateOrderRequest>()
            .Produces(200)
            .Produces(201)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<CreateOrderResponse>();

        private static async Task<IResult> Handle(CreateOrderRequest request, ISaleProcessingService _saleProcessing, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            CreateOrderResponse response = new CreateOrderResponse();
            try
            {
                string userId = "";//fetch from header
                var result = await _saleProcessing.CreateOrder(request, userId, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException ex)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ex.Message;
                _logger.LogError(message, ex);
                response.Success = false;
                response.Message = message;
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (ArgumentException ex)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = ex.Message;
                _logger.LogError(ex.Message, ex);
                response.Success = false;
                response.Message = message;
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (Exception ex)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, ex);
                response.Success = false;
                response.Message = message;
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
        }
    }
}
