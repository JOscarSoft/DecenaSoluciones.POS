using System.Net.Http.Headers;
using System.Net.Http;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Security.Claims;

namespace DecenaSoluciones.POS.Shared.Extensions
{
    public class JwtTokenHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorage _localStorage;

        public JwtTokenHeaderHandler(ILocalStorage localStorage)
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
