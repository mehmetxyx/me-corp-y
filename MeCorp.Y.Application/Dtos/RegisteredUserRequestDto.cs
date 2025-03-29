using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Application.Dtos;

public class RegisteredUserRequestDto
{
    public string Username { get; set; }
    public UserRole Role { get; set; }
    public string Password { get; set; }
    public string? ReferralCode { get; set; }
}