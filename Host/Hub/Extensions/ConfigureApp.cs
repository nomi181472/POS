using DA.AppDbContexts;
using Hub.HubGrpc.Server;
using Hub.Middlewares;
using MapConfig;
using Microsoft.EntityFrameworkCore;

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
            await app.EnsureDatabaseCreated();
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

        }

        private static async Task EnsureDatabaseCreated(this WebApplication app)
        {
            // using var scope = app.Services.CreateScope();
            // var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // await db.Database.MigrateAsync();

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                Console.WriteLine("Applying pending migrations...");
                await dbContext.Database.MigrateAsync(); // Apply pending migrations
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }
}
