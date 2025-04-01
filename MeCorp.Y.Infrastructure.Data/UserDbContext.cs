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

        modelBuilder.Entity<UserEntity>()
            .HasData(
                new UserEntity
                {
                    Id = 1,
                    CreatedAtUtc = DateTime.UtcNow,
                    Role = Domain.Enums.UserRole.Admin,
                    Username = "mehmet",
                    PasswordHash = "k75FPfxn177WTgOsJH251v3sLKFCy7rH0tA1Xq3bveIf1KxwSsxnaIKTOnkA67DSohFqwUwCJz4ByFKZuDhM3Q=="
                },
                new UserEntity
                {
                    Id = 2,
                    CreatedAtUtc = DateTime.UtcNow,
                    Role = Domain.Enums.UserRole.Admin,
                    Username = "mecorp",
                    PasswordHash = "k75FPfxn177WTgOsJH251v3sLKFCy7rH0tA1Xq3bveIf1KxwSsxnaIKTOnkA67DSohFqwUwCJz4ByFKZuDhM3Q=="
                }
            );
        modelBuilder.Entity<ReferralTokenEntity>()
            .HasData(
                new ReferralTokenEntity
                {
                    Id = 1,
                    Code = "CreateAsManager",
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
