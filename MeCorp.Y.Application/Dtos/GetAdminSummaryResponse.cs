using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Application.Dtos;

public class GetAdminSummaryResponse
{
    public int Id { get; internal set; }
    public string Username { get; internal set; }
    public UserRole Role { get; internal set; }
    public DateTime CreatedAtUtc { get; internal set; }
}