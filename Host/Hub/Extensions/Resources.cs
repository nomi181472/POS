using Logger;
using BS;

namespace Hub.Extensions
{
    public static class Resources
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddCustomLogger(configuration)
            .AddBusinessLayer(configuration)
            .AddSwagger();
            



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
