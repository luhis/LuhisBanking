using TrueLayerAccess;

namespace LuhisBanking.Services
{
    public class Tokens
    {
        public Tokens(string accessToken, string refreshToken)
        {
            AccessToken = accessToken.NotNullOrEmpty();
            RefreshToken = refreshToken.NotNullOrEmpty();
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}