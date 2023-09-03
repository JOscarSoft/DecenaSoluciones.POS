using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public class JwtTokenHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public JwtTokenHeaderHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("bearer"))
            {
                var userSession = await _localStorage.GetStorage<UserInfoExtension>("userSession");

                if (userSession != null && !string.IsNullOrWhiteSpace(userSession.Token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", userSession.Token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
