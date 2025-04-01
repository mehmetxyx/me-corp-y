namespace MeCorp.Y.Infrastructure.Data.UnitOfWorks;

public interface IUnitOfWork
{
    public void BeginTransaction();

    public Task SaveAsync();

    public void Rollback();
}