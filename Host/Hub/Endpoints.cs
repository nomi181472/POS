using Hub.Common;
using Hub.Common.Filters;
using Hub.Features.AreaCoverageManagement;

namespace Hub
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("")
                .AddEndpointFilter<RequestLoggingFilter>()
                .WithOpenApi();

            endpoints.MapAreaCoverageManagementEndpoints();

        }

        private static void MapAreaCoverageManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/AreaCoverageManagement")
                .WithTags("AreaCoverageManagement");

            endpoints.MapPublicGroup()
                //.MapEndpoint<Signup>()
                .MapEndpoint<AddArea>();
        }

        private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
        {
            return app.MapGroup(prefix ?? string.Empty)
                .AllowAnonymous();
        }

        private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
        {
            return app.MapGroup(prefix ?? string.Empty)
                .RequireAuthorization()
                .WithOpenApi();
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IFeature
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
