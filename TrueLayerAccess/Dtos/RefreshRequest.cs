namespace TrueLayerAccess.Dtos
{
    public class RefreshRequest
    {
        public RefreshRequest(string clientId, string clientSecret, string refreshToken)
        {
            this.ClientId = clientId.NotNullOrEmpty();
            this.ClientSecret = clientSecret.NotNullOrEmpty();
            this.RefreshToken = refreshToken.NotNullOrEmpty();
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RefreshToken { get; }
    }
}