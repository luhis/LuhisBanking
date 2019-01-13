using System.Threading.Tasks;
using LuhisBanking.Services;
using Microsoft.EntityFrameworkCore;

namespace LuhisBanking.Persistence
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Login> Logins { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupLoginTable.Setup(modelBuilder.Entity<Login>());
        }

        public async Task SeedData()
        {
            //// await this.Database.MigrateAsync();
        }
    }
}
