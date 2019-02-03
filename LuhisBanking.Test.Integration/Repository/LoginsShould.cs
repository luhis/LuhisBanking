using System;
using System.Threading;
using LuhisBanking.Persistence;
using LuhisBanking.Persistence.Repositories;
using LuhisBanking.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LuhisBanking.Test.Integration.Repository
{
    public class LoginsShould
    {
        private readonly ILoginsRepository loginsRepository;

        public LoginsShould()
        {
            var bk = new BankingContext(new DbContextOptionsBuilder<BankingContext>()
                .UseSqlite("Data Source=../../../../LuhisBanking.db").EnableSensitiveDataLogging().Options);
            this.loginsRepository = new LoginsRepository(bk);
        }

        [Fact]
        public void Update()
        {
            var id = Guid.NewGuid();
            var n = new Login(id, "aaaa", "bbbb", DateTime.UtcNow);
            loginsRepository.Add(n, CancellationToken.None).Wait();
            
            loginsRepository.Update(n, CancellationToken.None).Wait();
        }
    }
}
