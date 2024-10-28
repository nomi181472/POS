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
    public class DeleteRole : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPatch($"/{nameof(DeleteRole)}", Handle)
            .WithSummary("Soft Delete a Role")
            .WithRequestValidation<RequestDeleteRole>()
            .Produces(HTTPStatusCode200.Ok)
            .Produces(HTTPStatusCode400.NotFound)
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

        private static async Task<IResult> Handle(RequestDeleteRole request, IUserContext userContext, IRoleService roleService, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Ok;
            string message = "Success";
            try
            {
                var result = await roleService.DeleteRole(request, userContext.Data.UserId, cancellationToken);
              
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
