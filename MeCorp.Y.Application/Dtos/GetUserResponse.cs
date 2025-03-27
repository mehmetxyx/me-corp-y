
namespace MeCorp.Y.Application.Dtos;

public class GetUserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}