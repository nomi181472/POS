﻿

using BS.Services.CashManagementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DA;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BS.Services.OrderService;
using BS.Services.TillManagementService;

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





        return services;
    }
    
}

