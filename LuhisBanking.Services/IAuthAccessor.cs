namespace LuhisBanking.Services
{
    public interface IAuthAccessor
    {
        string GetAccessToken();
        string GetRefreshToken();
    }
}