﻿using System.Security.Claims;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public class UserInfoExtension
    {
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";

        public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.Name, Username),
            new (ClaimTypes.Role, Role),
        }, "DecenaSolucionesSession"));

        public static UserInfoExtension FromClaimsPrincipal(ClaimsPrincipal principal) => new()
        {
            Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            Role = principal.FindFirst(ClaimTypes.Role)?.Value ?? ""
        };
    }
}
