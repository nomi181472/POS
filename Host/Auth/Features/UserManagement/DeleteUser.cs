using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
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
            .WithSummary("Add Role Details")
            .WithRequestValidation<RequestDeleteUser>()
            .Produces(200)
            .Produces<ResponseAddRoleToUser>();

        public class RequestValidator : AbstractValidator<RequestDeleteUser>
        {
            IUserService _user;
            public RequestValidator(IUserService user)
            {
                _user = user;

                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("Role name is required.")
                    .Must(IsRoleIdExist).WithMessage("Role not found.");
            }
            private bool IsRoleIdExist(string userId)
            {
                return _user.IsUserExistByUserId(userId);
            }
        }

        private static async Task<IResult> Handle(RequestDeleteUser request, IUserService userService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await userService.DeleteUser(request, "", cancellationToken);
              
                var response = new ResponseAddRoleToUser();
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
