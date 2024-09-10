using AttendanceService.Common;
using Auth.Extensions.RouteHandler;
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
    public class DeleteRole : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPatch($"/{nameof(DeleteRole)}", Handle)
            .WithSummary("Delete Role Details")
            .WithRequestValidation<RequestDeleteRole>()
            .Produces(200)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestDeleteRole>
        {
            IRoleService _role;
            public RequestValidator(IRoleService roleService)
            {
                _role = roleService;

                RuleFor(x => x.RoleId)
                    .NotEmpty().WithMessage("Role name is required.")
                    .Must(IsRoleIdExist).WithMessage("Role not found.");
            }
            private bool IsRoleIdExist(string roleId)
            {
                return _role.IsRoleExistByRoleId(roleId);
            }
        }

        private static async Task<IResult> Handle(RequestDeleteRole request, IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                var result = await roleService.DeleteRole(request, "", cancellationToken);
              
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
