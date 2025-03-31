using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.Queries;
using MeCorp.Y.Shared;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public interface IUserRepository
{
    Task Add(User user);
    Task<AdminSummaryResult> GetAdminSummary();
    Task<Result<User>> GetUserById(int id);
    Task<Result<User>> GetUsersByUsername(string username);
    Task Update(User user);
}