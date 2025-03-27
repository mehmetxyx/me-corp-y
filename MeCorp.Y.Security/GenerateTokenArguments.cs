namespace MeCorp.Y.Infrastructure.Security;

public class GenerateTokenArguments
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}