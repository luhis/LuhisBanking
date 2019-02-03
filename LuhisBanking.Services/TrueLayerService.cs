using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly ILoginsRepository loginsRepository;

        public TrueLayerService(IOptions<MyAppSettings> myAppSettings, ILoginsRepository loginsRepository)
        {
            this.loginsRepository = loginsRepository;
            this.myAppSettings = myAppSettings.Value;
        }

        private static Login UpdateTokens(Login t, string access, string refresh) => new Login(t.Id, access, refresh, DateTime.UtcNow);

        private async Task<Login> ReAuthorise(Login login, CancellationToken cancellationToken)
        {
            var reAuth = await TrueLayerAuthApi.RenewAuthToken(new RefreshRequest(myAppSettings.ClientId,
                myAppSettings.ClientSecret,
                login.RefreshToken));
            var newLogin = UpdateTokens(login, reAuth.access_token, reAuth.refresh_token);
            await this.loginsRepository.Update(newLogin, cancellationToken);

            return newLogin;
        }

        private async Task<OneOf<(Login, T), Error>> RetryIfUnAuthorised<T>(Func<string, Task<OneOf<T, Unauthorised, Error>>> f,
            Login login, CancellationToken cancellationToken)
        {
            var res = await f(login.AccessToken);
            return await res.Match(
                some => Task.FromResult((OneOf<(Login, T), Error>) (login, some)),
                async unAuthorised =>
                {
                    var reAuth = await ReAuthorise(login, cancellationToken);
                    var r = await f(reAuth.AccessToken);
                    return r.Match(success => (OneOf<(Login, T), Error>) (login, success),
                        _ => throw new Exception("Failed to refresh auth"),
                        error => (OneOf<(Login, T), Error>) error);
                },
                error => Task.FromResult((OneOf<(Login, T), Error>) error));
        }

        async Task<IReadOnlyList<OneOf<(Login, Result<Account>), Error>>> ITrueLayerService.GetAccounts(CancellationToken cancellationToken)
        {
            var logins = await loginsRepository.GetAll(cancellationToken);
            var tasks = logins.Select(b => RetryIfUnAuthorised(TrueLayerApi.GetAllAccountsAsync, b, cancellationToken));
            return await Task.WhenAll(tasks);
        }

        Task<OneOf<(Login, Result<Balance>), Error>> ITrueLayerService.GetAccountBalance(Login login,
            string accountId, CancellationToken cancellationToken)
        {
            return RetryIfUnAuthorised(t => TrueLayerApi.GetAccountBalance(accountId, t), login, cancellationToken);
        }

        async Task<IReadOnlyList<OneOf<(Login, Result<MetaData>), Error>>> ITrueLayerService.GetLogins(CancellationToken cancellationToken)
        {
            var logins = await loginsRepository.GetAll(cancellationToken);
            var tasks = logins.Select(b => RetryIfUnAuthorised(TrueLayerApi.GetMetaData, b, cancellationToken));
            return await Task.WhenAll(tasks);
        }
    }
}
