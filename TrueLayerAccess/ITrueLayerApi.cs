using System.Threading.Tasks;
using TrueLayerAccess.Dtos;
using OneOf;
using MetaData = TrueLayerAccess.Dtos.MetaData;

namespace TrueLayerAccess
{
    public interface ITrueLayerApi
    {
        Task Delete(string accessToken);

        Task<OneOf<Result<MetaData>, Unauthorised, Error>> GetMetaData(string accessToken);

        Task<OneOf<Result<Balance>, Unauthorised, Error>> GetAccountBalance(string accountId, string accessToken);

        Task<OneOf<Result<Transaction>, Unauthorised, Error>> GetAccountTransactions(string accountId,
            string accessToken);

        Task<OneOf<Result<Account>, Unauthorised, Error>> GetAllAccountsAsync(string accessToken);
    }
}