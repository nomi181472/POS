using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.RoleService.Models.Request;
using BS.Services.UserService.Models;
using BS.Services.UserService.Models.Request;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.UserManagement
{
    public class UpdateUser : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(UpdateUser)}", Handle)
            .WithSummary("Update User Details")
            .WithRequestValidation<RequestUpdateUser>()
            .Produces(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestUpdateUser>
        {

            IUserService _service;
            public RequestValidator(IUserService service)
            {
                _service = service;

                RuleFor(n => n.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                    .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Name cannot be only whitespaces.");

                RuleFor(x => x.Email).EmailAddress().Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").NotEmpty();

                RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(IsUserIdExist).WithMessage("Invalid UserId");

            }
            private bool IsUserExist(string email)
            {
                return !_service.IsUserExist(email);

            }
            private bool IsUserIdExist(string userId)
            {
                return _service.IsUserExistByUserId(userId);
            }
        }

        private static async Task<IResult> Handle(RequestUpdateUser request, IUserContext userContext, IUserService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await service.UpdateUser(request, userContext.Data.UserId, cancellationToken);

               
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (InvalidOperationException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (ArgumentNullException e)
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
