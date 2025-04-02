using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Application.Dtos;

public class GetUserResponse
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required UserRole Role { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}