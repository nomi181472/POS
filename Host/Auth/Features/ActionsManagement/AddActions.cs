using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ActionsService;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.ActionsManagement
{
    public class AddActions : IActionsFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddActions)}", Handle)
            .WithSummary("Add Role Details")
            .WithRequestValidation<RequestAddAction>()
            .Produces(200)
            .Produces<ResponseAddAction>();

        public class RequestValidator : AbstractValidator<RequestAddAction>
        {
            public RequestValidator()
            {
                //RuleFor(x => x.Email).EmailAddress().NotEmpty();
            }
        }

        private static async Task<IResult> Handle(RequestAddAction request, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await actionService.AddAction(request, "", cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                var response = new ResponseAddAction();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch(RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch(Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
        }
    }
}
