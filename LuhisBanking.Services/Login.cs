using System;

namespace LuhisBanking.Services
{
    public class Login
    {
        private Login()
        {

        }

        public Login(Guid id, string accessToken, string refreshToken)
        {
            Id = id;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public Guid Id { get; private set; }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }
    }
}
