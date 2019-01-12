using LuhisBanking.Services;
using Microsoft.AspNetCore.Http;
using TrueLayerAccess;

namespace LuhisBanking.Server
{
    public class AuthAccessor : IAuthAccessor
    {
        private readonly IResponseCookies responseCookies;
        private readonly IRequestCookieCollection requestCookieCollection;

        public AuthAccessor(IRequestCookieCollection requestCookieCollection, IResponseCookies responseCookies)
        {
            this.responseCookies = responseCookies.NotNull();
            this.requestCookieCollection = requestCookieCollection.NotNull();
        }

        Tokens IAuthAccessor.GetTokens()
        {
            return new Tokens(requestCookieCollection.GetAccessToken(), requestCookieCollection.GetRefreshToken());
        }

        void IAuthAccessor.SetTokens(Tokens tokens)
        {
            responseCookies.SetAccessToken(tokens.AccessToken);
            responseCookies.SetRefreshToken(tokens.RefreshToken);
        }
    }
}