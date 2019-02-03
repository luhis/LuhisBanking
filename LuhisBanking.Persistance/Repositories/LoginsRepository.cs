using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Services;
using Microsoft.EntityFrameworkCore;

namespace LuhisBanking.Persistence.Repositories
{
    public class LoginsRepository : ILoginsRepository
    {
        private readonly BankingContext context;

        public LoginsRepository(BankingContext context)
        {
            this.context = context;
        }

        Task<IReadOnlyList<Login>> ILoginsRepository.GetAll(CancellationToken cancellationToken)
        {
            return context.Logins.ToReadOnlyAsync(cancellationToken);
        }

        Task<Login> ILoginsRepository.GetById(Guid id, CancellationToken cancellationToken)
        {
            return context.Logins.SingleAsync(a => a.Id == id, cancellationToken);
        }

        Task ILoginsRepository.Add(Login login, CancellationToken cancellationToken)
        {
            this.context.Logins.Add(login);
            return context.SaveChangesAsync(cancellationToken);
        }

        Task ILoginsRepository.Update(Login login, CancellationToken cancellationToken)
        {
            this.context.Logins.Update(login);
            return context.SaveChangesAsync(cancellationToken);
        }

        Task ILoginsRepository.Delete(Login login, CancellationToken cancellationToken)
        {
            this.context.Logins.Remove(login);
            return this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
