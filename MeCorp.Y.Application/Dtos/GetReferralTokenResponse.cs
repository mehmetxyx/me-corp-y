namespace MeCorp.Y.Application.Dtos;

public class GetReferralTokenResponse
{
    public string Code { get; internal set; }
    public int Id { get; internal set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public bool IsValid { get; internal set; }
}