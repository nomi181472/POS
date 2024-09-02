using MapConfig;
using Microsoft.AspNetCore.Diagnostics;

namespace Till.Extensions
{
    public static class ConfigureApp
    {
        public static async Task Configure(this WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.MapEndpoints();
            app.MapEndpointsExposed();
            //TODO: Add migration await app.EnsureDatabaseCreated();

        }
        public static async Task GlobalExceptionHandler(this WebApplication app)
        {
        }
        private static async Task EnsureDatabaseCreated(this WebApplication app)
        {
            // using var scope = app.Services.CreateScope();
            // var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // await db.Database.MigrateAsync();
        }
    }
}