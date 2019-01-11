using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using OneOf;
using TrueLayerAccess.Dtos;

namespace TrueLayerAccess
{
    public static class TrueLayerApi
    {
        private static HttpClient GetClient(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            return client;
        }

        private static async Task<OneOf<T, Unauthorised, Error>> Get<T>(string accessToken, string url)
        {
            var client = GetClient(accessToken);
            var result = await client.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new Unauthorised();
            }

            var json = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return Json.Deserialize<T>(json);
            }
            else
            {
                return Json.Deserialize<Error>(json);
            }
        }

        public static Task<OneOf<Result<Account>, Unauthorised, Error>> GetAllAccountsAsync(string accessToken)
        {
            return Get<Result<Account>>(accessToken, TrueLayerUris.GetAccountsUrl);
        }

        public static Task<OneOf<Result<Transaction>, Unauthorised, Error>> GetAccountTransactions(string accountId, string accessToken)
        {
            return Get<Result<Transaction>>(accessToken, TrueLayerUris.GetTransactionsUrl(accountId));
        }

        public static Task<OneOf<Result<Balance>, Unauthorised, Error>> GetAccountBalance(string accountId, string accessToken)
        {
            return Get<Result<Balance>>(accessToken, TrueLayerUris.GetBalanceUrl(accountId));
        }
    }
}