﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGatewayServices.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddHelpers(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IRead,Config>();

            return services;
        }

        
    }
}
