using MeCorp.Y.Api;

namespace MeCorp.Y.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDbContext userDbContext;

    public UnitOfWork(UserDbContext userDbContext)
    {
        this.userDbContext = userDbContext;
    }
    public void BeginTransaction()
    {
        //currently not needed
    }

    public void Rollback()
    {
        //currently not needed
    }

    public async Task SaveAsync()
    {
        await userDbContext.SaveChangesAsync();
    }
}
