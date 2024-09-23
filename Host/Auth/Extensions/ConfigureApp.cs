using Auth;
using Auth.Middlewares;
using DA.AppDbContexts;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
            app.UseMiddleware<UserContextMiddleware>();
            await app.EnsureDatabaseCreated();
            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

        }
        public static async Task GlobalExceptionHandler(this WebApplication app)
        {

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