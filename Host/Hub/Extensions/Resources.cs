using Logger;
using BS;
using UserActivity;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Hub.Common;
using Auth.Common.Constant;
using Helpers;
namespace Hub.Extensions
{
    public static class Resources
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddCustomLogger(configuration)
            .AddBusinessLayer(configuration)
            .AddUserActivityLogging(configuration)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UserActivityLogCommandHandler).Assembly))
            .AddSwagger()
            .AddEndpointGRPC(configuration, KConstant.ApiName, Assembly.GetExecutingAssembly(), typeof(IFeature)); ;
            



            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
                options.InferSecuritySchemes();
            });
            return services;

        }
    }
}
