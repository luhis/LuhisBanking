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
        private readonly ITrueLayerApi trueLayerApi;
        private readonly ITrueLayerAuthApi trueLayerAuthApi;

        public TrueLayerService(IOptions<MyAppSettings> myAppSettings, ILoginsRepository loginsRepository, ITrueLayerApi trueLayerApi, ITrueLayerAuthApi trueLayerAuthApi)
        {
            this.loginsRepository = loginsRepository;
            this.trueLayerApi = trueLayerApi;
            this.trueLayerAuthApi = trueLayerAuthApi;
            this.myAppSettings = myAppSettings.Value;
        }

        private async Task<Login> ReAuthorise(Login login, CancellationToken cancellationToken)
        {
            var reAuth = await trueLayerAuthApi.RenewAuthToken(new RefreshRequest(myAppSettings.ClientId,
                myAppSettings.ClientSecret,
                login.RefreshToken));
            login.UpdateTokens(reAuth.access_token, reAuth.refresh_token);
            await this.loginsRepository.Update(login, cancellationToken);

            return login;
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
            var tasks = logins.Select(b => RetryIfUnAuthorised(trueLayerApi.GetAllAccountsAsync, b, cancellationToken));
            return await Task.WhenAll(tasks);
        }

        Task<OneOf<(Login, Result<Balance>), Error>> ITrueLayerService.GetAccountBalance(Login login,
            string accountId, CancellationToken cancellationToken)
        {
            return RetryIfUnAuthorised(t => trueLayerApi.GetAccountBalance(accountId, t), login, cancellationToken);
        }

        async Task<IReadOnlyList<OneOf<(Login, Result<MetaData>), Error>>> ITrueLayerService.GetLogins(CancellationToken cancellationToken)
        {
            ////todo allow removing corrupted accounts
            var logins = await loginsRepository.GetAll(cancellationToken);
            var tasks = logins.Select(b => RetryIfUnAuthorised(trueLayerApi.GetMetaData, b, cancellationToken));
            return await Task.WhenAll(tasks);
        }

        public async Task DeleteLogin(Guid id, CancellationToken cancellationToken)
        {
            var login = await loginsRepository.GetById(id, cancellationToken);
            await trueLayerApi.Delete(login.AccessToken);
            await loginsRepository.Delete(login, cancellationToken);
        }
    }
}
