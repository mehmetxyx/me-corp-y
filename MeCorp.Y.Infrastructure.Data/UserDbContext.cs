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
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ReferralTokenEntity> ReferralTokens { get; set; }
}
