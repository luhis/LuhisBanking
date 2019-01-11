namespace TrueLayerAccess.Dtos
{
    public class TokenRequest
    {
        public TokenRequest(string clientId, string clientSecret, string code, string redirectUri)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Code = code;
            this.RedirectUri = redirectUri;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Code { get; }
        public string RedirectUri { get; }
    }
}