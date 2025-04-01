
namespace MeCorp.Y.Domain.DomainEntities;

public class BlockedIp
{
    public int Id { get; set; }
    public string? IpAddress { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime BlockUntil { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public bool IsBlocked { get => BlockUntil > DateTime.UtcNow; }

    public void IncreaseFailedLoginCount()
    {
        FailedLoginAttempts++;
        if (FailedLoginAttempts >= 10)
        {
            FailedLoginAttempts = 0;
            BlockUntil = DateTime.UtcNow.AddMinutes(1);
        }
    }
}