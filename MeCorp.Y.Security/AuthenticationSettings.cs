namespace MeCorp.Y.Infrastructure.Security;

public class AuthenticationSettings
{
    public required string SecretKey { get; set; }
    public required string HashKey { get; set; }
    public required string Audience { get; set; }
    public bool ValidateAudience { get; set; }
    public required string Issuer { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public required TimeSpan ExpirationTimeSpan { get; set; }
}
