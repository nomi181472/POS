using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.NotificationManagementService;
using BS.Services.NotificationManagementService.Models.Request;
using DM.DomainModels;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.NotificationManagement
{
    public class AddNotification : INotificationFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddNotification)}", Handle)
            .WithSummary("Add Notification Details")
            .WithRequestValidation<RequestAddNotification>()
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddNotification>
        {
            INotificationManagementService _service;
            public RequestValidator(INotificationManagementService service)
            {
                _service = service;

                RuleFor(t => t.Title)
                    .NotEmpty().WithMessage("Title is required")
                    .Must(title => !string.IsNullOrWhiteSpace(title)).WithMessage("Title cannot be only whitespaces.");

                RuleFor(d => d.Description)
                    .NotEmpty().WithMessage("Description is required")
                    .Must(description => !string.IsNullOrWhiteSpace(description)).WithMessage("Description cannot be only whitespaces.");

                RuleFor(m => m.Message)
                    .NotEmpty().WithMessage("Message is required")
                    .Must(message => !string.IsNullOrWhiteSpace(message)).WithMessage("Message cannot be only whitespaces.");

                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required")
                    .Must(userId => !string.IsNullOrWhiteSpace(userId)).WithMessage("UserId cannot be only whitespaces.");

                RuleFor(x => x.At)
                    .NotEmpty().WithMessage("At is required");
            }
        }

        private static async Task<IResult> Handle(RequestAddNotification request, IUserContext userContext, INotificationManagementService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await service.AddNotification(request, userContext.Data.UserId, cancellationToken);
                //var token = jwt.GenerateToken(new Common.JWT.UserPayload() { Id = result.UserId, RoleIds = result.RoleIds });
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (ArgumentException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
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
