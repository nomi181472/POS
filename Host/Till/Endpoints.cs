
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Till.Common.Auth;

namespace Till;

public static class Endpoints
{
    #region Auth
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
    #endregion Auth

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

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddCash>()
                 .MapEndpoint<ListCashWithDetails>()
                 .MapEndpoint<UpdateCash>()
                 ;
    }

    private static void MapPaymentMethodEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPaymentMethodFeature)}")
                           .WithTags("PaymentMethods");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<ListAllPaymentMethods>()
                 ;
    }

    private static void MapTillManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ITillFeature)}")
                           .WithTags("TillManagement");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddTill>()
                 .MapEndpoint<ListAllTills>()
                 ;
    }

    private static void MapOrderManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IOrderFeature)}")
                           .WithTags("OrderManagement");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddOrderDetails>()
                 .MapEndpoint<ListOrderDetailsWithDetails>()
                 .MapEndpoint<UpdateOrderDetails>()
                 ;
    }


    private static void MapInventoryManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IInventoryFeature)}")
                           .WithTags("InventoryManagement");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<GetInventory>()
                 .MapEndpoint<ReloadInventory>()
                 ;
    }

    private static void MapCustomerManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(ICustomerFeature)}")
                           .WithTags("CustomerManagement");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddCustomer>()
                 .MapEndpoint<UpdateCustomer>()
                 .MapEndpoint<ListCustomerWithDetails>()
                 .MapEndpoint<GetCustomerHistoryById>()
                 ;
    }

    private static void MapPaymentManagementEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"/{nameof(IPaymentManagementFeature)}")
                           .WithTags("PaymentManagement");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddSurchargeDiscount>();
    }

    private static void MapSaleProcessingEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ISaleFeature)}")
                           .WithTags("SalesProcessing");

        endpoints.MapAuthorizedGroup()
            .MapEndpoint<CreateCart>()
            .MapEndpoint<UpdateCart>()
            .MapEndpoint<RemoveCart>()
            .MapEndpoint<GetActiveCartsByTill>()
            .MapEndpoint<AddItemsToCart>()
            .MapEndpoint<CreateOrder>()
            .MapEndpoint<ViewCart>();
    }

    private static void MapCustomerFeedbackEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ICustomerFeedbackFeature)}")
                           .WithTags("CustomerFeedback");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddCustomerFeedback>();
    }

    private static void MapCashSessionEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup($"{nameof(ICashSessionFeature)}")
                           .WithTags("CashSession");

        endpoints.MapAuthorizedGroup()
                 .MapEndpoint<AddCashSession>()
                 .MapEndpoint<GetCashDetailsByCashSessionId>();
    }

    #region RouteGroup & Endpoint Builders
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
    #endregion RouteGroup & Endpoint Builders
}