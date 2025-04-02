using System.ComponentModel.DataAnnotations;

namespace MeCorp.Y.Application.Dtos;
public class LoginUserRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username contains invalid characters.")]
    public required string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters.")]
    public required string Password { get; set; }
}
