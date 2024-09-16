
using Till.Common;
using Till.Common.Filters;
using Microsoft.OpenApi.Models;
using Till.Features.CashManagement;
using Till.Feature.OrderManagement;
using Till.Feature.CashManagement;
using Till.Common.Constant;
using Till.Feature.TillManagement;
using Till.Feature.InventoryManagement;
using Till.Feature.CustomerManagement;
using Till.Feature.PaymentManagement;
using Till.Feature.SaleProcessing;
using Till.Feature.CartManagement;
using Till.Feature.PaymentMethod;
using Till.Feature.CustomerFeedbackManagement;
using Till.Feature.CashSessionManagement;

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
        endpoints.MapTillManagementEndpoints();
        endpoints.MapInventoryManagementEndpoints();
        endpoints.MapCustomerManagementEndpoints();
        //endpoints.MapPaymentManagementEndpoints();
        endpoints.MapSaleProcessingEndpoints();
        endpoints.MapPaymentMethodEndpoints();
        endpoints.MapCustomerFeedbackEndpoints();
        endpoints.MapCashSessionEndpoints();
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

    private static void MapPaymentMethodEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPaymentMethodFeature)}")
            .WithTags("PaymentMethods");

        endpoints.MapPublicGroup()
            .MapEndpoint<ListAllPaymentMethods>();
    }

    private static void MapTillManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ITillFeature)}")
            .WithTags("TillManagement");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddTill>()
            .MapEndpoint<ListAllTills>();
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


    private static void MapInventoryManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IInventoryFeature)}")
                           .WithTags("InventoryManagement");

        endpoints.MapPublicGroup()
            .MapEndpoint<GetInventory>();
    }

    private static void MapCustomerManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ICustomerFeature)}")
                           .WithTags("CustomerManagement");

        endpoints.MapPublicGroup()
                 .MapEndpoint<AddCustomer>()
                 .MapEndpoint<UpdateCustomer>()
                 .MapEndpoint<ListCustomerWithDetails>();
    }

    private static void MapPaymentManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPaymentManagementFeature)}")
                           .WithTags("PaymentManagement");

        endpoints.MapPublicGroup()
                 .MapEndpoint<AddSurchargeDiscount>();
    }

    private static void MapSaleProcessingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ISaleFeature)}")
            .WithTags("SalesProcessing");

        endpoints.MapPublicGroup()
            .MapEndpoint<CreateCart>()
            .MapEndpoint<UpdateCart>()
            .MapEndpoint<RemoveCart>()
            .MapEndpoint<GetActiveCartsByTill>()
            .MapEndpoint<AddItemsToCart>()
            .MapEndpoint<CreateOrder>();
    }

    private static void MapCustomerFeedbackEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ICustomerFeedbackFeature)}")
            .WithTags("CustomerFeedback");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddCustomerFeedback>();
    }

    private static void MapCashSessionEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ICashSessionFeature)}")
            .WithTags("CashSession");

        endpoints.MapPublicGroup()
            .MapEndpoint<AddCashSession>()
            .MapEndpoint<GetCashDetailsByCashSessionId>();
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