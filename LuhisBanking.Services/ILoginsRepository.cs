using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LuhisBanking.Services
{
    public interface ILoginsRepository
    {
        Task<IReadOnlyList<Login>> GetAll(CancellationToken cancellationToken);

        Task<Login> GetById(Guid id, CancellationToken cancellationToken);

        Task Add(Login login, CancellationToken cancellationToken);

        Task Update(Login login, CancellationToken cancellationToken);

        Task Delete(Login login, CancellationToken cancellationToken);
    }
}