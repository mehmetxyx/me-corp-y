namespace MeCorp.Y.Infrastructure.Security;

public class AuthenticationSettings
{
    public string SecretKey { get; set; }
    public string HashKey { get; set; }
    public string Audience { get; set; }
    public bool ValidateAudience { get; set; }
    public string Issuer { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public TimeSpan ExpirationTimeSpan { get; set; }
}
