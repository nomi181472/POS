

using BS.Services.CashManagementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DA;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BS.Services.OrderService;
using BS.Services.TillManagementService;
using BS.Services.CustomerManagementService;
using BS.Services.InventoryManagementService;
using BS.Services.PaymentManagementService;
using BS.Services.SaleProcessingService;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddDALayer(configuration)
        .AddServices();

        return services;
    }
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.TryAddScoped<ICashManagementService, CashManagementService>();
        services.TryAddScoped<IOrderDetailsService, OrderDetailsService>();
        services.TryAddTransient<ITillManagementService, TillManagementService>();
        services.TryAddScoped<ICustomerManagementService, CustomerManagementService>();
        services.TryAddScoped<IInventoryManagementService, InventoryManagementService>();
        services.TryAddScoped<IPaymentManagementService, PaymentManagementService>();
        services.TryAddTransient<ISaleProcessingService, SaleProcessingService>();







        return services;
    }    
}

