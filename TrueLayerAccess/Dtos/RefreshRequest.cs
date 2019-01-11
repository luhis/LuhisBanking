namespace TrueLayerAccess.Dtos
{
    public class RefreshRequest
    {
        public RefreshRequest(string clientId, string clientSecret, string refreshToken)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.RefreshToken = refreshToken;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RefreshToken { get; }
    }
}