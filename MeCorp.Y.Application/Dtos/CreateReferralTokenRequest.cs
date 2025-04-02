using System.ComponentModel.DataAnnotations;

namespace MeCorp.Y.Application.Dtos;

public class CreateReferralTokenRequest
{
    [StringLength(100, ErrorMessage = "Referral code must not exceed 100 characters.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Referral code contains invalid characters.")]
    public required string Code { get; set; }
}