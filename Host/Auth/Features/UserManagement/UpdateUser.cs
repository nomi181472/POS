using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
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

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                  
                    .Must(IsUserExist).WithMessage("Email already exists.");

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

        private static async Task<IResult> Handle(RequestUpdateUser request, IUserService service, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await service.UpdateUser(request, "", cancellationToken);

               
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
