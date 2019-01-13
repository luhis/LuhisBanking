using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Services;

namespace LuhisBanking.Persistence.Repositories
{
    public class LoginsRepository : ILoginsRepository
    {
        private readonly BankingContext client;

        public LoginsRepository(BankingContext client)
        {
            this.client = client;
        }

        public Task<IReadOnlyList<Login>> GetAll(CancellationToken cancellationToken)
        {
            return client.Logins.ToReadOnlyAsync(cancellationToken);
        }

        public Task Add(Login login)
        {
            throw new NotImplementedException();
        }
    }
}
