using BS.Services.CustomerManagementService.Models.Request;
using BS.Services.CustomerManagementService;
using BS.Services.SaleProcessingService.Models.Request;
using Logger;
using BS.Services.SaleProcessingService;
using FluentValidation;
using Till.Extensions.RouteHandler;
using PaymentGateway.API.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using Till.Common;
using BS.CustomExceptions.Common;

namespace Till.Feature.CartManagement
{
    public class CreateCart:ICartFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(CreateCart)}", Handle)
            .WithSummary("Create cart")
            .WithRequestValidation<CreateCartRequest>()
            .Produces(200)
            .Produces(201)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces(406)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<CreateCartRequest>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(CreateCartRequest request, ISaleProcessingService _saleProcessing, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                string userId = "";//fetch from header
                var result = await _saleProcessing.CreateCart(request, userId, cancellationToken);
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
            catch(RecordAlreadyExistException ex)
            {
                statusCode = HTTPStatusCode400.NotAcceptable;
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
