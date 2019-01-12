namespace TrueLayerAccess.Dtos
{
    public class TokenRequest
    {
        public TokenRequest(string clientId, string clientSecret, string code, string redirectUri)
        {
            this.ClientId = clientId.NotNullOrEmpty();
            this.ClientSecret = clientSecret.NotNullOrEmpty();
            this.Code = code.NotNullOrEmpty();
            this.RedirectUri = redirectUri.NotNullOrEmpty();
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Code { get; }
        public string RedirectUri { get; }
    }
}