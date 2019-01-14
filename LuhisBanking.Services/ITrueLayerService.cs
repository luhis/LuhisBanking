using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Services
{
    public interface ITrueLayerService
    {
        Task<IReadOnlyList<OneOf<(Login, Result<Account>), Error>>> GetAccounts();

        Task<OneOf<(Login, Result<Balance>), Error>> GetAccountBalance(Login login, string accountId);
    }
}