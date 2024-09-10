//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();




//app.MapGet("/", () => "Hello World!");

//app.Run();
using Hub.Extensions;
using UserActivity;


var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterService(builder.Configuration);
var app = builder.Build();
await app.Configure();

/*app.MapGet("/api/user-activity/{userId}", async (string userId, IUserActivity userActivity) =>
{
    var logs = await userActivity.GetActivityAsync(userId);
    return logs ?? "No logs found for this user.";
});*/

app.MapGet("/api/user-activity2/{userId}", async (string userId, IUserActivity userActivity) =>
{
    var logs = await userActivity.GetActivityAsync(userId);
    return logs.Any() ? Results.Ok(logs) : Results.NotFound("No logs found for this user.");
});

app.Run();
