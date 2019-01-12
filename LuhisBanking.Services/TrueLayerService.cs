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
            IAuthAccessor authAccessor)
        {
            var tokens = authAccessor.GetTokens();
            var res = await f(tokens.AccessToken);
            return await res.Match(
                some => Task.FromResult((OneOf<T, Error>) some),
                async unAuthorised =>
                {
                    var reAuth = await TrueLayerAuthApi.RenewAuthToken(new RefreshRequest(myAppSettings.ClientId,
                        myAppSettings.ClientSecret,
                        tokens.RefreshToken));
                    authAccessor.SetTokens(new Tokens(reAuth.access_token, reAuth.refresh_token));
                    var r = await f(reAuth.access_token);
                    return r.Match(success => (OneOf<T, Error>) success,
                        _ => throw new Exception("Failed to refresh auth"),
                        error => (OneOf<T, Error>) error);
                },
                error => Task.FromResult((OneOf<T, Error>) error));
        }

        Task<OneOf<Result<Account>, Error>> ITrueLayerService.GetAccounts(IAuthAccessor authAccessor)
        {
            return RetryIfUnAuthorised(TrueLayerApi.GetAllAccountsAsync, authAccessor);
        }

        Task<OneOf<Result<Balance>, Error>> ITrueLayerService.GetAccountBalance(IAuthAccessor authAccessor,
            string accountId)
        {
            return RetryIfUnAuthorised(t => TrueLayerApi.GetAccountBalance(accountId, t), authAccessor);
        }
    }
}
