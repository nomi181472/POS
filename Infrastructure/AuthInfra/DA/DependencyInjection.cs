using DA.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDALayer(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDbContext(configuration)
                .AddUOW();
            return services;
        }
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(configuration.GetSection("ConnectionStrings:db").Value)
                );
            return services;
        }
        public static IServiceCollection AddUOW(this IServiceCollection services)
        {



            services.TryAddScoped<IUnitOfWork, UnitOfWork>();



            return services;
        }
    }
}
