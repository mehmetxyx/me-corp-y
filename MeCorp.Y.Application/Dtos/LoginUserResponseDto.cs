using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Application.Dtos;

public class LoginUserResponseDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public required string Token { get; set; }
}