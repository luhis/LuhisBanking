namespace TrueLayerAccess
{
    public static class TrueLayerUris
    {
        public static string BaseUri => "truelayer.com/";
        public static string GetAuthTokenUrl => $"https://auth.{BaseUri}connect/token";
        public static string GetAccountsUrl => $"https://api.{BaseUri}data/v1/accounts";
        public static string GetMetaDataUrl => $"https://api.{BaseUri}data/v1/me";

        public static string GetDeleteUrl => $"https://api.{BaseUri}api/delete";

        public static string GetBalanceUrl(string accountId) => $"https://api.{BaseUri}data/v1/accounts/{accountId}/balance";
        public static string GetTransactionsUrl(string accountId) => $"https://api.{BaseUri}data/v1/accounts/{accountId}/transactions";
    }
}