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

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("User email is required.")
                    .Must(UseEmail).WithMessage("User already exists.");
                RuleFor(X => X.RoleIds)
                    .Must(AllValidIds)
                    .WithMessage("Invalid roleId");
                RuleFor(x => x.UserType)
                    .Must(ShouldNotBeSuperAdmin).WithMessage("Invalid UserType");

            }
            private bool ShouldNotBeSuperAdmin(string userType)
            {
                return userType.ToLower() != KDefinedRoles.SuperAdmin.ToLower();
            }
            private bool UseEmail(string UserName)
            {
                return !_service.IsUserExist(UserName);
            }
            private bool AllValidIds(List<string> roleIds)
            {
                return _roleService.IsRoleExistByRoleId(roleIds.ToArray());
            }
        }

        private static async Task<IResult> Handle(RequestAddUser request, IUserService UserService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await UserService.AddUser(request, "", cancellationToken);
              
               
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
