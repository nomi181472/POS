using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.StoreManagementService.Model.Request;
using BS.Services.StoreManagementService;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using Hub.Extensions.RouteHandler;
using Hub.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.CustomExceptions.Common;

namespace Hub.Features.StoreManagement
{
    public class DeleteStore : IStoreManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app

        .MapPatch($"/{nameof(DeleteStore)}", Handle)
        .WithSummary("Delete the store")
        .WithRequestValidation<RequestDeleteStore>()
        .Produces(200)
        .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestDeleteStore>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Address).NotEmpty();
                //RuleFor(x => x.Name).NotEmpty();

            }
        }

        private static async Task<IResult> Handle(RequestDeleteStore request, IStoreManagementService store, ICustomLogger _logger /*IUserActivity _activity*/, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await store.DeleteStore(request, cancellationToken);

                var response = new ResponseAddArea();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (UnknownException e)
            {

                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW + e.Message;
                _logger.LogError(message, e);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ExceptionMessage.SWW + e.Message;
                _logger.LogError(message, e);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
