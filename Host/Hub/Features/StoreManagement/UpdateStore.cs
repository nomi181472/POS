using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.StoreManagementService.Model.Request;
using BS.Services.StoreManagementService;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using Hub.Common;
using Hub.Extensions.RouteHandler;
using BS.CustomExceptions.Common;

namespace Hub.Features.StoreManagement
{
    public class UpdateStore : IStoreManagementFeature 
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateStore)}", Handle)
            .WithSummary("update store information")
            .WithRequestValidation<RequestUpdateStore>()
            .Produces(200)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestUpdateStore>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Address).NotEmpty();
                //RuleFor(x => x.Name).NotEmpty();

            }
        }

        private static async Task<IResult> Handle(RequestUpdateStore request, IStoreManagementService store, ICustomLogger _logger /*IUserActivity _activity*/, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await store.UpdateStore(request, cancellationToken);

                //string userId = "SampleUser";
                //string activity = $"User {userId} added a new area with name: {request.Name}";
                //await _activity.LogActivityAsync(userId, activity);

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
                message = e.Message;
                _logger.LogError(message, e);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
