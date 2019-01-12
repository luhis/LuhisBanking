namespace LuhisBanking.Services
{
    public interface IAuthAccessor
    {
        Tokens GetTokens();

        void SetTokens(Tokens tokens);
    }
}