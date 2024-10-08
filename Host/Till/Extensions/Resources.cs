﻿//namespace Till.Extensions
//{
//    public class Resources
//    {

//    }
//}

using System.Text;
using Logger;
using Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Till.Common.Constant;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Reflection;
using Till.Common;
using Till.Extensions.Validators;
using Till.Common.JWT;
using Till.Common.Auth;
using Till.Common.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Helpers.Auth.Models;
using Till.Middlewares;
namespace Till.Extensions
{
    public static class ConfigDI
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services
                    .AddCustomLogger(configuration)
                    .AddSwagger(KConstant.ApiName)
                    .AddMiddlewares()
                    //TODO: AddServicesLayers
                    //TODO: AddFluentValidation.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly)
                    .AddAuthDI(configuration)
                   
                    .AddBusinessLayer(configuration)
                    .AddHelpers(configuration)
                    .AddEndpointGRPC(configuration,KConstant.ApiName,Assembly.GetExecutingAssembly(),typeof(IFeature))
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

        private static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
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
                return (user) => jwt.GenerateToken(user);
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

        private static IServiceCollection AddSwagger(this IServiceCollection services, string pTitle)
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
        /*public static IServiceCollection AddAuthDI(this IServiceCollection services, IConfiguration configuration)
        {
            services
                  //.AddCustomValidator(configuration)
                  //.AddJwtValidator(configuration)
                    .AddCustomAuthorization();

            //services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            //services.AddTransient<Jwt>();

            return services;
        }*/

        //private static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        //{
        //    services.AddAuthorization(options =>
        //    {
        //        options.AddPolicy("CustomPolicy", policy =>
        //        {
        //            policy.RequireAssertion(context =>
        //            {
        //                // Ensure the Resource is an HttpContext
        //                if (context.Resource is HttpContext httpContext)
        //                {
        //                    var roleManager = httpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        //                    var userRoles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value);

        //                    foreach (var role in userRoles)
        //                    {
        //                        var roleEntity = roleManager.FindByNameAsync(role).Result;
        //                        if (roleEntity != null)
        //                        {
        //                            /*var rolePolicies = roleEntity.Policies; // Assuming roleEntity has a Policies property

        //                            if (rolePolicies.Any(p => p.Name == "RequiredPolicy"))
        //                            {
        //                                return true;
        //                            }*/
        //                            return true;
        //                        }
        //                    }
        //                }

        //                return false;
        //            });
        //        });
        //    });
        //    return services;
        //}
    }
}