using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Extensions
{
    public class AuthExtension : AuthenticationStateProvider
    {
        private readonly ILocalStorage _localStorage;
        private readonly ClaimsPrincipal _emptyClaim = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthExtension(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task UpdateSessionState(string token)
        {
            ClaimsPrincipal claimsPrincipal = _emptyClaim;

            if (!string.IsNullOrEmpty(token))
            {
                claimsPrincipal = CreateClaimsPrincipalFromToken(token);
                var userInfo = UserInfoExtension.FromClaimsPrincipal(claimsPrincipal, token);
                await _localStorage.SaveStorage("userSession", userInfo);
            }
            else
            {
                _localStorage.RemoveFromStorage("userSession");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var userSession = await _localStorage.GetStorage<UserInfoExtension>("userSession");

            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(_emptyClaim));

            var claimPrincipal = userSession.ToClaimsPrincipal();


            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }

        private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity();

            if (tokenHandler.CanReadToken(token))
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                identity = new(jwtSecurityToken.Claims, "JwtIdentity");
                identity.AddClaim(new Claim(ClaimTypes.Name, jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == "unique_name")?.Value ?? ""));
                foreach (var item in jwtSecurityToken.Claims.Where(p => p.Type == "role"))
                    identity.AddClaim(new Claim(ClaimTypes.Role, item.Value));
            }

            return new(identity);
        }
    }
}
