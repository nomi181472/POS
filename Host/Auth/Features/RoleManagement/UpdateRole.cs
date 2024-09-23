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
    public class UpdateRole : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPut($"/{nameof(UpdateRole)}", Handle)
            .WithSummary("Update Role Details")
            .WithRequestValidation<RequestUpdateRole>().
            Produces(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<ResponseAddRoleToUser>();

        public class RequestValidator : AbstractValidator<RequestUpdateRole>
        {
            IRoleService _role;
            public RequestValidator(IRoleService roleService)
            {
                _role = roleService;

                RuleFor(x => x.RoleName)
                    .NotEmpty().WithMessage("Role name is required.")
                    .Must(RoleNameNotExist).WithMessage("Role already exists.");
                
                RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.")
                .Must(IsRoleIdExist).WithMessage("Invalid roleId");

            }
            private bool RoleNameNotExist(string roleName)
            {
                return !_role.IsRoleExist(roleName);

            }
            private bool IsRoleIdExist(string roleId)
            {
                return _role.IsRoleExistByRoleId(roleId);
            }
        }

        private static async Task<IResult> Handle(RequestUpdateRole request, IUserContext userContext, IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await roleService.UpdateRole(request, userContext.Data.UserId, cancellationToken);
                
                var response = new ResponseAddRoleToUser();
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (RecordNotFoundException e)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = e.Message;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, null);
            }
            catch (InvalidOperationException e)
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
