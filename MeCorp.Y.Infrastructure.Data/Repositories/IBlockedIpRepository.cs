using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Shared;

namespace MeCorp.Y.Infrastructure.Data.Repositories;

public interface IBlockedIpRepository
{
    Task Add(BlockedIp blockedIp);
    Task<Result<BlockedIp>> GetBlockedIpByIpAddress(string? userIp);
    Task Update(BlockedIp blockedIp);
}