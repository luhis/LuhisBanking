using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Services
{
    public interface ITrueLayerService
    {
        Task<IReadOnlyList<(Login, OneOf<Result<Account>, Error>)>> GetAccounts(CancellationToken cancellationToken);

        Task<(Login, OneOf<Result<Balance>, Error>)> GetAccountBalance(Login login, string accountId, CancellationToken cancellationToken);

        Task<IReadOnlyList<(Login, OneOf<Result<MetaData>, Error>)>> GetLogins(CancellationToken cancellationToken);

        Task DeleteLogin(Guid id, CancellationToken cancellationToken);
    }
}