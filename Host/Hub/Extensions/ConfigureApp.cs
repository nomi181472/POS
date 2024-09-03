﻿using Hub.HubGrpc.Server;
using Hub.Middlewares;
using MapConfig;

namespace Hub.Extensions
{
    public static class ConfigureApp
    {
        public static async Task Configure(this WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseMiddleware<UserActivityLoggingMiddleware>();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.MapEndpoints();
            app.MapGrpcService<HubGrpcImpl>();
            app.MapEndpointsExposed();
            //TODO: Add migration await app.EnsureDatabaseCreated();

        }

        private static async Task EnsureDatabaseCreated(this WebApplication app)
        {
            // using var scope = app.Services.CreateScope();
            // var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // await db.Database.MigrateAsync();
        }
    }
}
