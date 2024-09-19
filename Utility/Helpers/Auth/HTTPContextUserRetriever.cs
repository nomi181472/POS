﻿using Helpers.Auth.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Auth
{
    public static class HTTPContextUserRetriever
    {
        public static string GetUserName(HttpContext httpContext)
        {
            var user = httpContext.User;
            var payload = GetUserPayloadFromClaims(user);
            return payload.UserId ?? string.Empty;
        }

        public static UserPayload GetUserPayloadFromClaims(ClaimsPrincipal user)
        {
            var userPayload = new UserPayload
            {
                Email = user.FindFirst(KAuthClaimTypes.Email)?.Value,
                UserId = user.FindFirst(KAuthClaimTypes.UserId)?.Value,
                UserType = user.FindFirst(KAuthClaimTypes.UserType)?.Value,
                RoleIds = user.FindFirst(KAuthClaimTypes.Resources)?.Value
            };

            return userPayload;
        }

        public static class KAuthClaimTypes
        {
            public static string UserId { get; set; } = nameof(UserId);
            public static string UserType { get; set; } = nameof(UserType);
            public static string Resources { get; set; } = nameof(Resources);
            public static string Email { get; set; } = nameof(Email);
        }


    }
}
