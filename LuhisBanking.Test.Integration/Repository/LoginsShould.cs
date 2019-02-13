using System;
using System.Threading;
using FluentAssertions;
using LuhisBanking.Persistence;
using LuhisBanking.Persistence.Repositories;
using LuhisBanking.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LuhisBanking.Test.Integration.Repository
{
    public class LoginsShould
    {
        private static BankingContext GetContext()
        {
            return new BankingContext(new DbContextOptionsBuilder<BankingContext>()
                .UseSqlite("Data Source=../../../../LuhisBanking.db").EnableSensitiveDataLogging().Options);
        }

        [Fact]
        public void Update()
        {
            var id = Guid.NewGuid();
            using (var context = GetContext())
            {
                var r = (ILoginsRepository) new LoginsRepository(context);
                var n = new Login(id, "aaaa", "bbbb", DateTime.UtcNow);
                r.Add(n, CancellationToken.None).Wait();
           
                n.UpdateTokens("cccc", "dddd");
                r.Update(n, CancellationToken.None).Wait();

                var updated = r.GetById(id, CancellationToken.None).Result;
                updated.AccessToken.Should().BeEquivalentTo("cccc");
            }
        }
    }
}
