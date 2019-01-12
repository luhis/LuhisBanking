using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OneOf;
using TrueLayerAccess;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Services
{
    public class TrueLayerService : ITrueLayerService
    {
        private readonly MyAppSettings myAppSettings;

        public TrueLayerService(IOptions<MyAppSettings> myAppSettings)
        {
            this.myAppSettings = myAppSettings.Value;
        }

        private async Task<OneOf<T, Error>> RetryIfUnAuthorised<T>(Func<string, Task<OneOf<T, Unauthorised, Error>>> f,
            Tokens tokens)
        {
            var res = await f(tokens.AccessToken);
            return await res.Match(
                some => Task.FromResult((OneOf<T, Error>) some),
                async unAuthorised =>
                {
                    var reAuth = await TrueLayerAuthApi.RenewAuthToken(new RefreshRequest(myAppSettings.ClientId,
                        myAppSettings.ClientSecret,
                        tokens.RefreshToken));
                    var r = await f(reAuth.access_token);
                    return (OneOf<T, Error>) r.AsT0;
                },
                error => Task.FromResult((OneOf<T, Error>) error));
        }
        
        Task<OneOf<Result<Account>, Error>> ITrueLayerService.GetAccounts(Tokens tokens)
        {
            return RetryIfUnAuthorised(TrueLayerApi.GetAllAccountsAsync, tokens);
        }

        Task<OneOf<Result<Balance>, Error>> ITrueLayerService.GetAccountBalance(Tokens tokens, string accountId)
        {
            return RetryIfUnAuthorised(t => TrueLayerApi.GetAccountBalance(accountId, t), tokens);
        }
    }
}
