using MeCorp.Y.Api;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.PersistenceEntities;
using MeCorp.Y.Shared;
using Microsoft.EntityFrameworkCore;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public class BlockedIpRepository : IBlockedIpRepository
{
    private readonly UserDbContext dbContext;

    public BlockedIpRepository(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(BlockedIp blockedIp)
    {
        await dbContext.BlockedIps
            .AddAsync(blockedIp.ToEntity());
    }

    public async Task<Result<BlockedIp>> GetBlockedIpByIpAddress(string? userIp)
    {
        var blockedIpEntity = await dbContext.BlockedIps
            .Where(b => b.IpAddress == userIp)
            .FirstOrDefaultAsync();

        if (blockedIpEntity is null)
            return new Result<BlockedIp> { IsSuccessful = false };

        return new Result<BlockedIp>
        {
            IsSuccessful = true,
            Value = blockedIpEntity.ToDomainEntity()
        };
    }

    public async Task Update(BlockedIp blockedIp)
    {
        var entity = await dbContext.BlockedIps
            .Where(b => b.Id == blockedIp.Id)
            .FirstOrDefaultAsync();


        if (entity is null)
            return;

        entity.IpAddress = blockedIp.IpAddress;
        entity.BlockUntil = blockedIp.BlockUntil;
        entity.FailedLoginAttempts = blockedIp.FailedLoginAttempts;
    }
}
