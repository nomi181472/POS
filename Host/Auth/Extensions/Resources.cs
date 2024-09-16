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
using Auth.Common;
using System.Reflection;
using Auth.Common.Auth;
using Auth.Common.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Helpers.Auth.Models;
using NATSNotificationSystem;
using FluentValidation;
using Auth.Features.RoleManagement;
using BS.Services.RoleService.Models.Request;
using System.Xml;
using Helpers.ServiceCollectionExtensions;
using Auth.Features.UserManagement;
namespace ConfigResource
{
    public static class ConfigDI
    {
       
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var featureType = typeof(IFeature);
            string validatorName = "RequestValidator";
            services


            .AddCustomLogger(configuration)
            .AddSwagger(KConstant.ApiName)
            //TODO: AddServicesLayers
            .AddAuthDI(configuration)
            .AddBusinessLayer(configuration)
            .AddHelpers(configuration)
            .AddNatsService(configuration)
            .AddValidatorUsingAssemblies( assemblies, featureType, validatorName,typeof(IValidator<>))
            .AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });



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
          
            .AddJwtValidator(configuration)
            .AddCustomAuthorization();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddTransient<Jwt>();

            return services;
        }

        private static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {

            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
               // options.AddPolicy(KPolicyDescriptor.SuperAdminPolicy, policy=>policy.RequireAuthenticatedUser());
                options.AddPolicy(KPolicyDescriptor.CustomPolicy, policy => policy.RequireAuthenticatedUser()
                .AddRequirements(new CustomAuthorizationRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddSingleton<Func<UserPayload, AccessAndRefreshTokens>>(sp =>
            {
                var jwt = sp.GetRequiredService<Jwt>();
                return (user) => jwt.GenerateToken (user);
            });
            /* services.AddAuthorization(options =>
             {
                 options.AddPolicy(KPolicyDescriptor.SuperAdminPolicy, policy =>
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
                                     *//*var rolePolicies = roleEntity.Policies; // Assuming roleEntity has a Policies property

                                     if (rolePolicies.Any(p => p.Name == "RequiredPolicy"))
                                     {
                                         return true;
                                     }*//*
                                     return true;
                                 }
                             }
                         }

                         return false;
                     });
                 });
             });*/
            return services;
        }
    }
}
