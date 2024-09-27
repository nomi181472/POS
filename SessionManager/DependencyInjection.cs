using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRedisSessionManagement(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = ConfigurationOptions.Parse(redisConnectionString, true);
                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddScoped<RedisSessionManager>();

            return services;
        }
    }
}
