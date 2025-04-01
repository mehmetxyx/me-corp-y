using MeCorp.Y.Domain.DomainEntities;

namespace MeCorp.Y.Infrastructure.Data.PersistenceEntities;

public class BlockedIpEntity
{
    public int Id { get; set; }
    public string? IpAddress { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime BlockUntil { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public static class BlockedIpExtensions
{
    public static BlockedIpEntity ToEntity(this BlockedIp domain)
    {
        return new BlockedIpEntity
        {
            Id = domain.Id,
            IpAddress = domain.IpAddress,
            FailedLoginAttempts = domain.FailedLoginAttempts,
            BlockUntil = domain.BlockUntil,
            CreatedAtUtc = domain.CreatedAtUtc
        };
    }

    public static BlockedIp ToDomainEntity(this BlockedIpEntity entity)
    {
        return new BlockedIp
        {
            Id = entity.Id,
            IpAddress = entity.IpAddress,
            FailedLoginAttempts = entity.FailedLoginAttempts,
            BlockUntil = entity.BlockUntil,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }
}