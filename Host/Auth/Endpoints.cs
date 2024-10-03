
using Auth.Common;
using Auth.Common.Auth;
using Auth.Common.Constant;
using Auth.Common.Filters;
using Auth.Features.ActionsManagement;
using Auth.Features.AuthManagement;
using Auth.Features.RoleManagement;
using Auth.Features.RouteManagement;
using Auth.Features.UserManagement;
using BS.Services.RoleService.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Auth;

public static class Endpoints
{
  
    private static readonly OpenApiSecurityScheme securityScheme = new()
    {
        Type = SecuritySchemeType.Http,
        Name = JwtBearerDefaults.AuthenticationScheme,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new()
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(KConstant.ApiName)
            .AddEndpointFilter<RequestLoggingFilter>()
            .WithOpenApi();

        endpoints.MapAuthEndpoints();
        endpoints.MapRoleManagementEndpoints();
        endpoints.MapActionsManagementEndpoints();
        endpoints.MapUserManagement();
    }
    private static void MapUserManagement(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IUserManagementFeature)}")
            .WithTags("UserManagement");

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<AddUser>()
            .MapEndpoint<DeleteUser>()
            .MapEndpoint<GetUser>()
            .MapEndpoint<GetUserDetailsWithActions>()
            .MapEndpoint<ListUsers>()
            .MapEndpoint<UpdateUser>()
            .MapEndpoint<GetTotalUsers>();
    }
    private static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IAuthFeature)}")
            .WithTags("Authentication");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignUp>()
            .MapEndpoint<Login>()
            .MapEndpoint<RefreshToken>()
            .MapEndpoint<ForgetPassword>()
            .MapEndpoint<ChangePassword>();
    }
    private static void MapRoleManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IRoleManagementFeature)}")
            .WithTags("Role");

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<AddRole>()
            .MapEndpoint<AddRoleToUser>()
            .MapEndpoint<DeleteRole>()
              .MapEndpoint<GetPoliciesByRoleId>()
            .MapEndpoint<GetRole>()
            .MapEndpoint<ListRoles>()
            .MapEndpoint<UpdateRole>()
            .MapEndpoint<ListRolesWithUsers>()
            .MapEndpoint<DetachUserRole>()
            .MapEndpoint<DetachUserRoles>()
            .MapEndpoint<DetachUserRoleByUserId>()
            .MapEndpoint<DetachUserRolesByUserId>()
            .MapEndpoint<ListRolesWithActions>()
            .MapEndpoint<GetRbacMatrix>();
    }
    private static void MapActionsManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IActionsFeature)}")
            .WithTags("Actions");

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<AddActionsInRole>()
            .MapEndpoint<AddActions>()
            .MapEndpoint<AppendActionTag>()
            .MapEndpoint<DeleteAction>()
            .MapEndpoint<DeleteActions>()
            .MapEndpoint<GetAllAction>()
            .MapEndpoint<GetOverallActions>()
            .MapEndpoint<RemoveActionTag>()
            .MapEndpoint<GetActionById>()
            .MapEndpoint<GetActionsDetailsById>();
    }
    
   
   

    

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .RequireAuthorization(KPolicyDescriptor.CustomPolicy)
            .WithOpenApi(x => new(x)
            {
                Security = [new() { [securityScheme] = [] }],
            });
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IFeature
    {
        
        TEndpoint.Map(app);
        return app;
    }
}