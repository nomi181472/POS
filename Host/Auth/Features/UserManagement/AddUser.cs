using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.UserService.Models;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models;
using DA.Common.CommonRoles;
using Auth.Middlewares;

namespace Auth.Features.UserManagement
{
    public class AddUser : IUserManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddUser)}", Handle)
            .WithSummary("Add User Details")
            .WithRequestValidation<RequestAddUser>()
            .Produces(HTTPStatusCode200.Created)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddUser>
        {
            IUserService _service;
            IRoleService _roleService;
            public RequestValidator(IUserService service,IRoleService roleService)
            {
                _service = service;
                _roleService = roleService;

                RuleFor(n => n.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                    .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Name cannot be only whitespaces.");
                RuleFor(x => x.Email).EmailAddress().Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").NotEmpty();
                RuleFor(m => m.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
                RuleFor(X => X.RoleIds)
                    .Must(AllValidIds)
                    .WithMessage("Invalid roleId");
                RuleFor(x => x.UserType)
                    .Must(UserType => !string.IsNullOrWhiteSpace(UserType)).WithMessage("UserType cannot be only whitespaces.")
                    .Must(ShouldNotBeSuperAdmin).WithMessage("Invalid UserType");

            }
            private bool ShouldNotBeSuperAdmin(string userType)
            {
                try
                {
                    return userType.ToLower() != KDefinedRoles.SuperAdmin.ToLower();
                }
                catch (NullReferenceException ex)
                {
                    return false;
                }
            }
            private bool UseEmail(string UserName)
            {
                try
                {
                    return !_service.IsUserExist(UserName);
                }
                catch (NullReferenceException ex)
                {
                    return false;
                }
            }
            private bool AllValidIds(List<string> roleIds)
            {
                try
                {
                    return _roleService.IsRoleExistByRoleId(roleIds?.ToArray() ?? []);
                }
                catch (NullReferenceException ex)
                {
                    return false;
                }
            }
        }

        private static async Task<IResult> Handle(RequestAddUser request, IUserContext userContext, IUserService UserService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await UserService.AddUser(request, userContext.Data.UserId, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (InvalidOperationException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (RecordAlreadyExistException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(true, false, message, statusCode, null);
            }
            catch (InvalidDataException e)
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
