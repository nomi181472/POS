using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InventoryManagementService;
using BS.Services.InventoryManagementService.Model.Request;
using BS.Services.InventoryManagementService.Model.Response;
using FluentValidation;
using Hub.Common;
using Hub.Extensions.RouteHandler;
using Logger;
using PaymentGateway.API.Common;

namespace Hub.Features.InventoryManagment
{
    public class AddItemGroup : IInventoryManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app

            .MapPost($"/{nameof(AddItemGroup)}", Handle)
            .WithSummary("Get area coverage data")
            .WithRequestValidation<RequestAddItemGroup>()
            .Produces(200)
            .Produces<ResponseAddItemGroup>();

        public class RequestValidator : AbstractValidator<RequestAddItemGroup>
        {
            //IInventoryManagementService _inventory;
            //public RequestValidator(IInventoryManagementService inventory)
            //{
            //    _inventory = inventory;

            //    RuleFor(x => x.Items.)
            //        .NotEmpty().WithMessage("Role name is required.")
            //        .Must(ItemGroupExist).WithMessage("Role already exists.");
            //}
            //private bool ItemGroupExist(int code)
            //{
            //    return !_inventory.IsItemGroupExist(code);
            //}
        }

        private static async Task<IResult> Handle(RequestAddItemGroup request, IInventoryManagementService area, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await area.AddItemGroup(request, "Dummyuser", cancellationToken);

                //string userId = "SampleUser";
                //string activity = $"User {userId} added a new area with name: {request.Name}";
                //await _activity.LogActivityAsync(userId, activity);

                var response = new ResponseAddItemGroup();
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
