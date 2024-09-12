using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.AreaCoverageManagementService;
using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using BS.Services.InventoryManagementService;
using BS.Services.InventoryManagementService.Model.Request;
using FluentValidation;
using Hub.Common;
using Hub.Extensions.RouteHandler;
using Hub.Features.AreaCoverageManagement;
using Logger;
using PaymentGateway.API.Common;
using System.Reflection.Metadata;

namespace Hub.Features.InventoryManagment
{
    public class AddItemData : IInventoryManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app

            .MapPost($"/{nameof(AddItemData)}", Handle)
            .WithSummary("Get area coverage data")
            .WithRequestValidation<RequestAddItemData>()
            .Produces(200)
            .Produces<ResponseAddItemData>();

        //public class RequestValidator : AbstractValidator<RequestAddArea>
        //{
        //    public RequestValidator()
        //    {
        //        RuleFor(x => x.Address).NotEmpty();
        //        RuleFor(x => x.Name).NotEmpty();

        //    }
        //}

        private static async Task<IResult> Handle(RequestAddItemData request, IInventoryManagementService area, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await area.AddItemData(request, "Dummyuser", cancellationToken);

                //string userId = "SampleUser";
                //string activity = $"User {userId} added a new area with name: {request.Name}";
                //await _activity.LogActivityAsync(userId, activity);

                var response = new ResponseAddItemData();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception e)
            {

                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);

                //string userId = "SampleUser";
                //string activity = $"User {userId} failed to add a new area due to an error.";
                //await _activity.LogActivityAsync(userId, activity);

                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
