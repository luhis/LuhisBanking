using System;
using LuhisBanking.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuhisBanking.Server
{
    public static class DIModule
    {
        private static DbContextOptions GetDbOptions(IServiceProvider a) => new DbContextOptionsBuilder<BankingContext>()
            .UseSqlite(a.GetService<IConfiguration>().GetSection("DbPath").Get<string>()).Options;

        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<DbContextOptions>(GetDbOptions);
        }
    }
}