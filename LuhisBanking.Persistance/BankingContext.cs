using System.Threading.Tasks;
using LuhisBanking.Persistence.Setup;
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

        public Task SeedData()
        {
            return this.Database.EnsureCreatedAsync();
        }
    }
}
