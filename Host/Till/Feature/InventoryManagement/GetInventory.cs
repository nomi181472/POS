using Till.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.InventoryManagementService;
using BS.Services.OrderService;
using BS.Services.OrderService.Models.Request;
using BS.Services.OrderService.Models.Response;
using FluentValidation;
using Logger;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Common;
using System.Reflection.Metadata;
using Till.Extensions.RouteHandler;
using Till.Feature.OrderManagement;

namespace Till.Feature.InventoryManagement
{
    public class GetInventory : IInventoryFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapGet($"/{nameof(GetInventory)}", Handle)
            .WithSummary("Fetches inventory")
            .Produces(200);

        //public class RequestValidator : AbstractValidator<RequestAddOrderDetails>
        //{
        //    public RequestValidator()
        //    {
        //        //RuleFor(x => x.Email).EmailAddress().NotEmpty();
        //    }
        //}

        private static async Task<IResult> Handle(string request, IInventoryManagementService inventory, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await inventory.GetInventoryData(request, cancellationToken);
                var response = new ResponseAddOrderDetails();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
