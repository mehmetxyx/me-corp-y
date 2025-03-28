using MeCorp.Y.Infrastructure.Data.PersistenceEntities;
using Microsoft.EntityFrameworkCore;

namespace MeCorp.Y.Api;

public class UserDbContext: DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        :base(options)
    {
    }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ReferralTokenEntity> ReferralTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Property(user => user.Role)
            .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}
