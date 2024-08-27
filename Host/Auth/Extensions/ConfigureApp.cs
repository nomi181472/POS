using Auth;
using Microsoft.AspNetCore.Diagnostics;

namespace MapConfig
{
    public static class ConfigureApp
    {
        public static async Task Configure(this WebApplication app)
        {
            //app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapEndpoints();
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