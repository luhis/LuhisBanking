﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Services;

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

        Task ILoginsRepository.Add(Login login)
        {
            this.context.Logins.Add(login);
            return context.SaveChangesAsync();
        }

        Task ILoginsRepository.Update(Login login)
        {
            this.context.Logins.Update(login);
            return context.SaveChangesAsync();
        }
    }
}
