using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Application.Dtos;

public class LoginUserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string Token { get; set; }
}