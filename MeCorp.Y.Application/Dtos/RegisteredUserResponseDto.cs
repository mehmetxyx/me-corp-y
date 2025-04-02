using MeCorp.Y.Domain.Enums;

public class RegisteredUserResponseDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}