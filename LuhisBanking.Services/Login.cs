using System;

namespace LuhisBanking.Services
{
    public class Login
    {
        private Login()
        {

        }

        public Login(Guid id, string accessToken, string refreshToken, DateTime modified)
        {
            Id = id;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Modified = modified;
        }

        public Guid Id { get; private set; }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime Modified { get; private set; }
    }
}
