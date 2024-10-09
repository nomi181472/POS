using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.StoreManagementService;
using BS.Services.StoreManagementService.Model.Response;
using Hub.Common;
using Logger;
using PaymentGateway.API.Common;

namespace Hub.Features.StoreManagement
{
    public class ListAllStore : IStoreManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
        .MapGet($"/{nameof(ListAllStore)}", Handle)
        .WithSummary("List All Store")
        .Produces(HTTPStatusCode200.Ok)
        .Produces(HTTPStatusCode400.Forbidden)
        .Produces<ResponseListAllStore>();



        private static async Task<IResult> Handle(IStoreManagementService store, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await store.ListAllStore(cancellationToken);

                var response = new ResponseListAllStore();
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
