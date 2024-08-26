using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public static class DependencyInjection
    {
        private static IConfiguration? Configuration;
        public static IServiceCollection AddCustomLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomLogger>((options =>
            new LoggerImpl(
              new LoggerConfiguration()
                   .WriteTo.File("logs/app-Logs.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger()
            )));

            Configuration = configuration;


            return services;
        }

        public static string SlackWebhook
        {
            get
            {
                if (Configuration==null||Configuration["SlackWebHook"] == null || string.IsNullOrWhiteSpace(Configuration["SlackWebHook"]))
                    throw new Exception("Slack Webhook is not valid in AppSettings file");

                return Configuration["SlackWebHook"]!;
            }
        }
    }
}
