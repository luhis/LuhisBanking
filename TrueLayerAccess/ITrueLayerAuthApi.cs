using System.Threading.Tasks;
using TrueLayerAccess.Dtos;

namespace TrueLayerAccess
{
    public interface ITrueLayerAuthApi
    {
        Task<TokenResponse> GetAuthToken(TokenRequest req);
        Task<TokenResponse> RenewAuthToken(RefreshRequest req);
        string AuthLink(string redirect, string clientId);
    }
}