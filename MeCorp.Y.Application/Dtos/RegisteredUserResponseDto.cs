using MeCorp.Y.Domain.Enums;

public class RegisteredUserResponseDto
{
    public int Id { get; internal set; }
    public string Username { get; internal set; }
    public UserRole Role { get; internal set; }
    public DateTime CreatedAtUtc { get; internal set; }
}