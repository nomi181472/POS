﻿using AttendanceService.Common;
using Auth.Common;
using Auth.Common.Constant;
using Auth.Extensions.RouteHandler;
using Auth.Middlewares;
using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.ExternalServices.GrpcClients;
using BS.Services.RoleService.Models;
using BS.Services.RoleService.Models.Request;
using BS.Services.RoleService.Models.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using System.Reflection;

namespace Auth.Features.RoleManagement
{
    public class AddActionsInRole : IRoleManagementFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(AddActionsInRole)}", Handle)
            .WithSummary("update Role Details")
            .WithRequestValidation<RequestAddActionsInRole>()
            .Produces(HTTPStatusCode200.Created)
            .Produces(HTTPStatusCode400.BadRequest)
            .Produces(HTTPStatusCode400.Forbidden)
            .Produces<bool>();

        public class RequestValidator : AbstractValidator<RequestAddActionsInRole>
        {
            IRoleService _role;
            public RequestValidator(IRoleService roleService)
            {
                _role = roleService;

                RuleFor(x => x.RoleId)
                    .NotEmpty().WithMessage("RoleId is required.")
                    .Must(IsRoleIdExist).WithMessage("Role not found");
            }
            private bool IsRoleIdExist(string roleId)
            {
                return _role.IsRoleExistByRoleId(roleId);
            }
        }

        private static async Task<IResult> Handle(RequestAddActionsInRole request, IUserContext userContext, IRoleService roleService,IActionControllerService action, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            try
            {
                #region Setup API and Assembly Details
                string ApiBaseName = KConstant.ApiName;
                Assembly assembly = Assembly.GetExecutingAssembly();
                var iFeature = typeof(IFeature);
                #endregion Setup API and Assembly Details

                #region Retrieve Actions from API
                var actions = await action.GetAllActionsOfAllApis(ApiBaseName, assembly, iFeature, cancellationToken);
                var allActions = new HashSet<string>(actions.SelectMany(x => x.Routes).Select(x => x.ToLower()));
                #endregion

                #region Validations
                bool hasNullAction = request.Actions.Any(x => string.IsNullOrWhiteSpace(x));
                var isAllValid = !hasNullAction && request.Actions.All(x => allActions.Contains(x.ToLower()));
                if (!isAllValid)
                {
                    statusCode = HTTPStatusCode400.NotFound;
                    message = "Invalid actions";
                    return ApiResponseHelper.Convert(true, false, message, statusCode, isAllValid);
                }
                #endregion Validations


                var result = await roleService.AddActionsInRole(request, userContext.Data.UserId, cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, result);
            }
            catch (ArgumentNullException e)
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
