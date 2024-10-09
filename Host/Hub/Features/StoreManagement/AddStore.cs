using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.AreaCoverageManagementService;
using FluentValidation;
using Hub.Features.AreaCoverageManagement;
using Logger;
using PaymentGateway.API.Common;
using BS.Services.StoreManagementService.Model.Request;
using BS.Services.StoreManagementService;
using Hub.Common;
using Hub.Extensions.RouteHandler;
using BS.CustomExceptions.CustomExceptionMessage;

namespace Hub.Features.StoreManagement
{
    public class AddStore : IStoreManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app

            .MapPost($"/{nameof(AddStore)}", Handle)
            .WithSummary("Add new store")
            .WithRequestValidation<RequestAddStore>()
            .Produces(200)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddStore>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Address).NotEmpty();
                //RuleFor(x => x.Name).NotEmpty();

            }
        }

        private static async Task<IResult> Handle(RequestAddStore request, IStoreManagementService store, ICustomLogger _logger /*IUserActivity _activity*/, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await store.AddStore(request, cancellationToken);

                //string userId = "SampleUser";
                //string activity = $"User {userId} added a new area with name: {request.Name}";
                //await _activity.LogActivityAsync(userId, activity);

                var response = new ResponseAddArea();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception e)
            {

                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW + e.Message;
                _logger.LogError(message, e);

                //string userId = "SampleUser";
                //string activity = $"User {userId} failed to add a new area due to an error.";
                //await _activity.LogActivityAsync(userId, activity);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
