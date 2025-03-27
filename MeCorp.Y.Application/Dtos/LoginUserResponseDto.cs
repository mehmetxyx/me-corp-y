namespace MeCorp.Y.Application.Dtos;

public class LoginUserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string Token { get; set; }
}