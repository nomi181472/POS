using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Auth.Common;
using Auth.Extensions.RouteHandler;
using AuthJWT;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Auth.Features.AuthManagement
{
   public class Login : IFeature
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handle)
        .WithSummary("Login a user")
        .WithRequestValidation<Request>();

    public record Request(string Username, string Password);
    public record Response(string Token);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(Request request, Jwt jwt, CancellationToken cancellationToken)
    {

        //TODO: login here
        // var user = new Common.JWT.UserPayload(){Id="1"};


        // if (user is null || user.Password != request.Password)
        // {
        //     return TypedResults.Unauthorized();
        // }

        var token = jwt.GenerateToken(new Common.JWT.UserPayload(){Id="1"});
        var response = new Response(token);
        return TypedResults.Ok(response);
    }
}
}