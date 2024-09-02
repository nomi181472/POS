
using Auth.Common;
using Auth.Common.Constant;
using Auth.Common.Filters;

using Auth.Features.AuthManagement;
using Auth.Features.RouteManagement;
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

        endpoints.MapAuthenticationEndpoints();
        endpoints.MapToExposedRoutes();
        
    }

    private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IAuthFeature)}")
            .WithTags("Authentication");

        endpoints.MapPublicGroup()
            .MapEndpoint<SignUp>()
            .MapEndpoint<Login>();
    }
    private static void MapToExposedRoutes(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IActionController)}")
            .WithTags("Routes");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetAllEndpoints>();
    }

   
   

    

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
            .RequireAuthorization()
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