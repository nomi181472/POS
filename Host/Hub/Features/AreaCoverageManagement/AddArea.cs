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

namespace Hub.Features.AreaCoverageManagement
{
    public class AddArea: IFeature
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

        private static async Task<IResult> Handle(RequestAddArea request, IAreaCoverageManagementService area, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await area.AddArea(request, "", cancellationToken);
                var response = new ResponseAddArea();
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
