using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using TrueLayerAccess.Dtos;

namespace TrueLayerAccess
{
    public static class TrueLayerAuthApi
    {
        private static string JoinWithEscapedSpaces(IEnumerable<string> s) => string.Join("%20", s);

        public static string AuthLink(string redirect, string clientId)
        {
            var scopes = new[] {"info", "accounts", "balance", "offline_access"};
            return
                $"https://auth.{TrueLayerUris.BaseUri}?" + 
                $"response_type=code&client_id={clientId}&nonce=1461411507&scope={JoinWithEscapedSpaces(scopes)}&" + 
                $"redirect_uri={redirect}&enable_oauth_providers=false&enable_open_banking_providers=true&enable_credentials_sharing_providers=false";
        }

        private static async Task<TokenResponse> Post(IEnumerable<KeyValuePair<string, string>> data)
        {
            var client = new HttpClient();

            var x = await client.PostAsync(TrueLayerUris.GetAuthTokenUrl, new FormUrlEncodedContent(data));
            x.ThrowIfNotSuccess();

            var json = await x.Content.ReadAsStringAsync();

            return Json.Deserialize<TokenResponse>(json);
        }

        public static Task<TokenResponse> GetAuthToken(TokenRequest req)
        {
            return Post(new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"client_id", req.ClientId},
                {"client_secret", req.ClientSecret},
                {"redirect_uri", req.RedirectUri},
                {"code", req.Code}
            });
        }

        public static Task<TokenResponse> RenewAuthToken(RefreshRequest req)
        {
            return Post(new Dictionary<string, string>
            {
                {"grant_type", "refresh_token"},
                {"client_id", req.ClientId},
                {"client_secret", req.ClientSecret},
                {"refresh_token", req.RefreshToken},
            });
        }
    }
}