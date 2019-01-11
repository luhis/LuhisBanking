namespace TrueLayerAccess.Dtos
{
    public class TokenResponse
    {
        public TokenResponse()
        {
        }

        public TokenResponse(string access_token, int expiresIn, string refresh_token, string token_type)
        {
            this.access_token = access_token;
            this.expires_in = expiresIn;
            this.refresh_token = refresh_token;
            this.token_type = token_type;
        }

        public string access_token { get; set; }
        
        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string token_type { get; set; }
    }
}