﻿

using BS.ExternalServices.GrpcClients;
using BS.ExternalServices.SourceGenerators;
using BS.Services.ActionsService;
using BS.Services.AdminDashboardService;
using BS.Services.AuthService;
using BS.Services.RoleService.Models;
using BS.Services.UserService.Models;
using DA;
using Microsoft.CodeAnalysis;
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
        services.TryAddScoped<IRoleService, RoleService>();
        services.TryAddScoped<IUserService, UserService>();
        services.TryAddScoped<IActionControllerService, ActionControllerService>();
        services.TryAddScoped<IIncrementalGenerator, ApiRouteGenerator>();
        services.TryAddScoped<IActionService, ActionService>();
        services.TryAddScoped<IAdminDashboardService, AdminDashboardService>();

        return services;
    }
    
}

