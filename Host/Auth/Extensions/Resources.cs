using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Auth.Common.Validators;
using Logger;
using AuthJWT;
namespace ConfigResource
{
    public static class ConfigDI
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services
            .AddCustomLogger(configuration)
            .AddSwagger()
            //TODO: AddServicesLayers
            //TODO: AddFluentValidation.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly)
            .AddAuthDI(configuration);
            


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
        public static IServiceCollection AddAuthDI(this IServiceCollection services, IConfiguration configuration)
        {



            services
            .AddCustomValidator(configuration)
            .AddJwtValidator(configuration);
           




            services.AddAuthorization();
          

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddTransient<Jwt>();
            
            return services;
        }


    }
}
