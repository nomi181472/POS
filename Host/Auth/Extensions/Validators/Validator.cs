using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthJWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Common.Validators
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        [Obsolete]
        public CustomAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Your custom authentication logic here
            var isValid = ValidateLocalAuth();

            if (!isValid)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            var claims = new[] { new Claim(ClaimTypes.Name, "LocalUser") };
            var identity = new ClaimsIdentity(claims, "LocalAuthIssuer");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "LocalAuthIssuer");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private bool ValidateLocalAuth()
        {
            // Implement your local authentication logic
            return true; // Placeholder for real validation
        }
    }
    public static class Validator
    {
        public static IServiceCollection AddCustomValidator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "LocalAuthIssuer";
                options.DefaultChallengeScheme = "LocalAuthIssuer";
            })
            .AddScheme<AuthenticationSchemeOptions, CustomAuthHandler>("LocalAuthIssuer", null);

            return services;
        }
        public static IServiceCollection AddJwtValidator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = Jwt.SecurityKey(configuration["Jwt:Key"]!),
                    ValidateIssuer = false,//TODO will be added in future
                    ValidateAudience = false, // TODO will be added in future
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
               
                };
            });

            return services;
        }
    }
}