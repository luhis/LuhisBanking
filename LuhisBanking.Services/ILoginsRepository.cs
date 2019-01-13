using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LuhisBanking.Services
{
    public interface ILoginsRepository
    {
        Task<IReadOnlyList<Login>> GetAll(CancellationToken cancellationToken);

        Task Add(Login login);
    }
}