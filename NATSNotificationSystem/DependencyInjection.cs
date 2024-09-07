using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NATSNotificationSystem
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNatsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<INatsService, NatsService>();

            return services;
        }
    }
}
