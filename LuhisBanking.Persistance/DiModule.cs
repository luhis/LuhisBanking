using LuhisBanking.Persistence.Repositories;
using LuhisBanking.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LuhisBanking.Persistence
{
    public static class DIModule
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<ILoginsRepository, LoginsRepository>();
            services.AddSingleton<BankingContext>();
        }
    }
}