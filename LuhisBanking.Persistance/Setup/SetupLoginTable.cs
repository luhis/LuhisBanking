using LuhisBanking.Services;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuhisBanking.Persistence.Setup
{
    public static class SetupLoginTable
    {
        public static void Setup(EntityTypeBuilder<Login> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedNever();
            entity.Property(e => e.AccessToken).IsRequired();
            entity.Property(e => e.RefreshToken).IsRequired();
        }
    }
}