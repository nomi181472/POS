using Auth.Common.Constant;
using Hub.Common;
using Hub.Common.Filters;
using Hub.Features.AreaCoverageManagement;
using Hub.Features.InventoryManagment;

namespace Hub
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup($"{KConstant.ApiName}")
                .AddEndpointFilter<RequestLoggingFilter>()
                .WithOpenApi();

            endpoints.MapAreaCoverageManagementEndpoints();
            endpoints.MapInventoryManagementEndpoints();

        }

        private static void MapAreaCoverageManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup($"/{nameof(IAreaCoverageManagementFeature)}")
                .WithTags("AreaCoverageManagement");

            endpoints.MapPublicGroup()
                //.MapEndpoint<Signup>()
                .MapEndpoint<AddArea>();
        }

        private static void MapInventoryManagementEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup($"/{nameof(IInventoryManagementFeature)}")
                .WithTags("InventoryManagemnt");

            endpoints.MapPublicGroup()
                .MapEndpoint<AddItemData>();

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
