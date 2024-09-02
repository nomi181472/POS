using FluentValidation;
using Hub.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Hub.Extensions.RouteHandler;
using BS.Services.AreaCoverageManagementService.Model.Request;
using BS.Services.AreaCoverageManagementService.Model.Response;
using Logger;
using BS.Services.AreaCoverageManagementService;
using PaymentGateway.API.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using UserActivity;
using System.Diagnostics;

namespace Hub.Features.AreaCoverageManagement
{
    public class AddArea: IAreaCoverageManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app

            .MapPost($"/{nameof(AddArea)}", Handle)
            .WithSummary("Get area coverage data")
            .WithRequestValidation<RequestAddArea>()
            .Produces(200)
            .Produces<ResponseAddArea>();

        public class RequestValidator : AbstractValidator<RequestAddArea>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.Name).NotEmpty();

            }
        }

        private static async Task<IResult> Handle(RequestAddArea request, IAreaCoverageManagementService area, ICustomLogger _logger /*IUserActivity _activity*/, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await area.AddArea(request, "", cancellationToken);

                //string userId = "SampleUser";
                //string activity = $"User {userId} added a new area with name: {request.Name}";
                //await _activity.LogActivityAsync(userId, activity);

                var response = new ResponseAddArea();
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
