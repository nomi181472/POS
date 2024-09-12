using BS.Services.AreaCoverageManagementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Services.InventoryManagementService;

namespace BS
{
    public static class DependancyInjection
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


            services.TryAddScoped<IAreaCoverageManagementService, AreaCoverageManagementService>();
            services.TryAddScoped<IInventoryManagementService, InventoryManagementService>();








            return services;
        }
    }
}
