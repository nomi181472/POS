

using BS.ExternalServices.GrpcClients;
using BS.Services.AuthService;
using DA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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


        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<IActionControllerService, ActionControllerService>();








        return services;
    }
    
}

