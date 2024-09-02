using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Auth.Common.Validators;
using Logger;
using AuthJWT;
using Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Auth.Common.Constant;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace ConfigResource
{
    public static class ConfigDI
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services


            .AddCustomLogger(configuration)
            .AddSwagger(KConstant.ApiName)
            //TODO: AddServicesLayers
            //TODO: AddFluentValidation.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly)
            .AddAuthDI(configuration)
            .AddBusinessLayer(configuration)
            .AddHelpers(configuration);
            
            


            return services;
        }
       
      
        private static IServiceCollection AddSwagger(this IServiceCollection services,string pTitle)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = pTitle,
                    Version = "v1",
                });
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
                options.InferSecuritySchemes();
            });
            return services;

        }
        public static IServiceCollection AddAuthDI(this IServiceCollection services, IConfiguration configuration)
        {



            services
            .AddCustomValidator(configuration)
            .AddJwtValidator(configuration)
            .AddCustomAuthorization();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddTransient<Jwt>();

            return services;
        }

        private static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomPolicy", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        // Ensure the Resource is an HttpContext
                        if (context.Resource is HttpContext httpContext)
                        {
                            var roleManager = httpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
                            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value);

                            foreach (var role in userRoles)
                            {
                                var roleEntity = roleManager.FindByNameAsync(role).Result;
                                if (roleEntity != null)
                                {
                                    /*var rolePolicies = roleEntity.Policies; // Assuming roleEntity has a Policies property

                                    if (rolePolicies.Any(p => p.Name == "RequiredPolicy"))
                                    {
                                        return true;
                                    }*/
                                    return true;
                                }
                            }
                        }

                        return false;
                    });
                });
            });
            return services;
        }
    }
}
