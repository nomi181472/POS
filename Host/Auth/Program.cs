using ConfigResource;
using MapConfig;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.RegisterService(builder.Configuration);
    var app = builder.Build();
    await app.Configure();
    app.Run();
}
catch (Exception ex)
{
    //TODO: call logger
    Console.WriteLine($"Application terminated unexpectedly:{ex.Message}");
}
finally
{
    //TODO: Logger resource closing
    Console.WriteLine("Fially");
}
