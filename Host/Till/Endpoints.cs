
using Till.Common;
using Till.Common.Filters;
using Microsoft.OpenApi.Models;
using Till.Features.CashManagement;
using Till.Feature.OrderManagement;
using Till.Feature.CashManagement;
using Till.Common.Constant;

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
        var endpoints = app.MapGroup(KConstant.ApiName)
                           .AddEndpointFilter<RequestLoggingFilter>()
                           .WithOpenApi();

        endpoints.MapCashManagementEndpoints();
        endpoints.MapOrderManagementEndpoints();
        //endpoints.MapToExposedRoutes();
    }

    private static void MapCashManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ICashFeature)}")
                           .WithTags("CashManagement");

        endpoints.MapPublicGroup()
                 .MapEndpoint<AddCash>()
                 .MapEndpoint<ListCashWithDetails>()
                 .MapEndpoint<UpdateCash>();
    }

    private static void MapOrderManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IOrderFeature)}")
                           .WithTags("OrderManagement");

        endpoints.MapPublicGroup()
                 .MapEndpoint<AddOrderDetails>()
                 .MapEndpoint<ListOrderDetailsWithDetails>()
                 .MapEndpoint<UpdateOrderDetails>();
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