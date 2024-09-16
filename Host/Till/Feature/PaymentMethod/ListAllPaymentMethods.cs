using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.PaymentMethodService;
using BS.Services.PaymentMethodService.Models.Response;
using BS.Services.SaleProcessingService.Models.Request;
using Logger;
using PaymentGateway.API.Common;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Till.Common;
using Till.Feature.CartManagement;

namespace Till.Feature.PaymentMethod
{
    public class ListAllPaymentMethods:IPaymentMethodFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(ListAllPaymentMethods)}", Handle)
            .WithSummary("List all payment methods.")
            .Produces(200)
            .Produces(201)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<List<PaymentMethodsResponse>>();

        private static async Task<IResult> Handle(IPaymentMethodsService _paymentMethods, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            List<PaymentMethodsResponse> response = new List<PaymentMethodsResponse>();
            try
            {
                response = await _paymentMethods.ListAllPaymentMethods(cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, response);
            }
            catch (RecordNotFoundException ex)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (ArgumentNullException ex)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (Exception ex)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
        }
    }
}
