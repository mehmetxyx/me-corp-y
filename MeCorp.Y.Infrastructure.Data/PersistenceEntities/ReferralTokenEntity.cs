namespace MeCorp.Y.Infrastructure.Data.PersistenceEntities;

public class ReferralTokenEntity
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public bool IsValid { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}