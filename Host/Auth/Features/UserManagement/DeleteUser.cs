using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.RoleService.Models;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using BS.Services.UserService.Models;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.UserManagement
{
    public class DeleteUser : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPatch($"/{nameof(DeleteUser)}", Handle)
            .WithSummary("Add User Details")
            .WithRequestValidation<RequestDeleteUser>()
            .Produces(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<ResponseAddRoleToUser>();

        public class RequestValidator : AbstractValidator<RequestDeleteUser>
        {
            IUserService _user;
            public RequestValidator(IUserService user)
            {
                _user = user;

                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("User name is required.")
                    .Must(IsRoleIdExist).WithMessage("User not found.");
            }
            private bool IsRoleIdExist(string userId)
            {
                return _user.IsUserExistByUserId(userId);
            }
        }

        private static async Task<IResult> Handle(RequestDeleteUser request, IUserContext userContext, IUserService userService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await userService.DeleteUser(request, userContext.Data.UserId, cancellationToken);
                var response = new ResponseAddRoleToUser();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (InvalidOperationException e)
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
