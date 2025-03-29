using MeCorp.Y.Infrastructure.Data.PersistenceEntities;
using Microsoft.EntityFrameworkCore;

namespace MeCorp.Y.Api;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ReferralTokenEntity> ReferralTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Property(user => user.Role)
            .HasConversion<string>();

        modelBuilder.Entity<ReferralTokenEntity>()
            .HasData(
                new ReferralTokenEntity
                {
                    Id = 1,
                    Code = "CreateAsAdmin",
                    CreatedAtUtc = DateTime.UtcNow,
                    IsValid = true
                },
                new ReferralTokenEntity
                {
                    Id = 2,
                    Code = "FromLinkedin",
                    CreatedAtUtc = DateTime.UtcNow,
                    IsValid = true
                }
            );

        base.OnModelCreating(modelBuilder);
    }
}
