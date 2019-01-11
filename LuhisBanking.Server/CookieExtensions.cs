using Microsoft.AspNetCore.Http;

namespace WebApplication1.Server
{
    public static class CookieExtensions
    {
        private static string AccessToken => "access_token";
        private static string RefreshToken => "renewal_token";

        public static void SetAccessToken(this IResponseCookies cookies, string accessToken)
        {
            cookies.Append(AccessToken, accessToken);
        }

        public static void SetRefreshToken(this IResponseCookies cookies, string renewalToken)
        {
            cookies.Append(RefreshToken, renewalToken);
        }

        public static string GetAccessToken(this IRequestCookieCollection cookies)
        {
            return cookies[AccessToken];
        }

        public static string GetRenewalToken(this IRequestCookieCollection cookies)
        {
            return cookies[RefreshToken];
        }
    }
}