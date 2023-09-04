using Blazored.LocalStorage;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public class AuthExtension : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _emptyClaim = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthExtension(ILocalStorageService localStorage)
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
                await _localStorage.RemoveItemAsync("userSession");
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
