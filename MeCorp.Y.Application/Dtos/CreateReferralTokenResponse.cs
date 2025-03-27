
namespace MeCorp.Y.Application.Dtos;

public class CreateReferralTokenResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public bool IsValid { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}