using System.Threading.Tasks;
using OneOf;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Services
{
    public interface ITrueLayerService
    {
        Task<OneOf<Result<Account>, Error>> GetAccounts(IAuthAccessor authAccessor);

        Task<OneOf<Result<Balance>, Error>> GetAccountBalance(IAuthAccessor authAccessor, string accountId);
    }
}