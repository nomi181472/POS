﻿
using Till.Common;
using Till.Common.Filters;
using Microsoft.OpenApi.Models;
using Till.Features.CashManagement;

namespace Till;

public static class Endpoints
{
    //private static readonly OpenApiSecurityScheme securityScheme = new()
    //{
    //    Type = SecuritySchemeType.Http,
    //    Name = JwtBearerDefaults.AuthenticationScheme,
    //    Scheme = JwtBearerDefaults.AuthenticationScheme,
    //    Reference = new()
    //    {
    //        Type = ReferenceType.SecurityScheme,
    //        Id = JwtBearerDefaults.AuthenticationScheme
    //    }
    //};

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("")
                           .AddEndpointFilter<RequestLoggingFilter>()
                           .WithOpenApi();

        endpoints.MapAuthenticationEndpoints();

    }

    private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/CashManagement")
                           .WithTags("CashManagement");

        endpoints.MapPublicGroup()
                 .MapEndpoint<AddCash>();
    }

    private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
    {
        return app.MapGroup(prefix ?? string.Empty)
                  .AllowAnonymous();
    }

    //private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
    //{
    //    return app.MapGroup(prefix ?? string.Empty)
    //        .RequireAuthorization()
    //        .WithOpenApi(x => new(x)
    //        {
    //            Security = [new() { [securityScheme] = [] }],
    //        });
    //}

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IFeature
    {
        TEndpoint.Map(app);
        return app;
    }
}