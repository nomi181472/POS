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

            // Replace IConnectionMultiplexer in RedisSessionManager.cs with ConnectionMultiplexer
            services.AddSingleton<IConnectionMultiplexer>(x =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints = { "redis_container:6379" }
                });

                return multiplexer;
            });

            services.AddScoped<RedisSessionManager>();

            return services;
        }
    }
}
