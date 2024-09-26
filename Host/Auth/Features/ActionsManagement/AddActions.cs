using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.ActionsService;
using BS.Services.ActionsService.Models.Request;
using BS.Services.ActionsService.Models.Response;
using FluentValidation;
using Helpers.Auth;
using Logger;
using Microsoft.AspNetCore.Http;
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
            IActionService _actionService;
            public RequestValidator(IActionService actionService)
            {
                _actionService = actionService;

                RuleFor(x => x.Name)
                    .Must(IsActionAvailable).WithMessage("Action already available")
                    .NotEmpty().WithMessage("Name is required.")
                    .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                    .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Name cannot be only whitespaces.");

            }
            public bool IsActionAvailable(string name)
            {
                return !_actionService.IsActionsAvailable(name);
            }
        }

        private static async Task<IResult> Handle(RequestAddAction request, IUserContext userContext, IActionService actionService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await actionService.AddAction(request, userContext.Data.UserId, cancellationToken);
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
            catch (ArgumentNullException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
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
