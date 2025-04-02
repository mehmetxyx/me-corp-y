using MeCorp.Y.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MeCorp.Y.Application.Dtos;

public class RegisteredUserRequestDto
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Username contains invalid characters.")]
    public required string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters.")]
    public required string Password { get; set; }

    [StringLength(100, ErrorMessage = "Referral code must not exceed 10 characters.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Referral code contains invalid characters.")]
    public string? ReferralCode { get; set; }
}