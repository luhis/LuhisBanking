using System.Threading.Tasks;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Services
{
    public interface ITrueLayerService
    {
        Task<OneOf<Result<Account>, Error>> GetAccounts(Tokens tokens);

        Task<OneOf<Result<Balance>, Error>> GetAccountBalance(Tokens tokens, string accountId);
    }
}