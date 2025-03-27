using MeCorp.Y.Api;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.PersistenceEntities;
using MeCorp.Y.Shared;
using Microsoft.EntityFrameworkCore;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext dbContext;

    public UserRepository(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<Result<User>> Add(User user)
    {
        var userEntity = user.ToEntity();
        await dbContext.Users.AddAsync(userEntity);

        return new Result<User>
        {
            IsSuccessful = true, 
            Value = userEntity.ToDomainEntity()
        };
    }

    public async Task<Result<User>> GetUserById(int id)
    {
        var userEntity = await dbContext.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (userEntity is null)
            return new Result<User> { Message = $"Can not find user id {id}" };

        return new Result<User>
        {
            IsSuccessful = true,
            Value = userEntity.ToDomainEntity()
        };
    }

    public async Task<Result<User>> GetUsersByUsername(string username)
    {
        var userEntity = await dbContext.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();

        if (userEntity is null)
            return new Result<User> { Message = $"user {username} not found." };


        return new Result<User>
        {
            IsSuccessful = true,
            Value = userEntity.ToDomainEntity()
        };
    }

    public async Task<Result<List<User>>> GetAdminSummary()
    {
        var users = await dbContext.Users
            .Where(u => u.Role == "Admin")
            .ToListAsync();


        if (users is null || users.Count == 0)
            return new Result<List<User>> { Message = "No Admin users found!" };

        var domainUsers = users.Select(u => u.ToDomainEntity()).ToList();
        return new Result<List<User>> 
        { 
            IsSuccessful = true,
            Value = domainUsers
        };
    }

    public async Task Update(User user)
    {
        var userEntity = await dbContext.Users
            .Where(u => u.Id == user.Id)
            .FirstOrDefaultAsync();

        userEntity.FailedLoginAttempts = user.FailedLoginAttempts;
        //dbContext.Entry(userEntity).State = EntityState.Modified;
    }
}
