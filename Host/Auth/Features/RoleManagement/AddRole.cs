using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.RoleService.Models;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;

namespace Auth.Features.RoleManagement
{
    public class AddRole : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddRole)}", Handle)
            .WithSummary("update Role Details")
            .WithRequestValidation<RequestAddRole>()
            .Produces(HTTPStatusCode200.Created)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddRole>
        {
            IRoleService _role;
            public RequestValidator(IRoleService roleService)
            {
                _role = roleService;

                RuleFor(x => x.RoleName)
                    .NotEmpty().WithMessage("Role name is required.")
                    .Must(RoleNameNotExist).WithMessage("Role already exists.");
            }
            private bool RoleNameNotExist(string roleName)
            {
                return !_role.IsRoleExist(roleName);
            }
        }

        private static async Task<IResult> Handle(RequestAddRole request, IUserContext userContext, IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await roleService.AddRole(request, userContext.Data.UserId, cancellationToken);
              
                var response = new ResponseAddRoleToUser();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentException e)
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
