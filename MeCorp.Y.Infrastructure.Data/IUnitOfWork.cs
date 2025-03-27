namespace MeCorp.Y.Infrastructure.Data;

public interface IUnitOfWork
{
    public void BeginTransaction();

    public Task SaveAsync();

    public void Rollback();
}