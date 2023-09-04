﻿using System.Security.Claims;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public class UserInfoExtension
    {
        public string Token { get; set; } = "";
        public string Username { get; set; } = "";
        public List<string> Roles { get; set; } = new();

        public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.Name, Username)
        }.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray()), "JwtIdentity"));

        public static UserInfoExtension FromClaimsPrincipal(ClaimsPrincipal principal, string token) => new()
        {
            Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            Token = token,
            Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
        };
    }
}