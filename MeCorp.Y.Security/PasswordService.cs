using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

namespace MeCorp.Y.Infrastructure.Security;

public class PasswordService : IPasswordService
{
    private readonly ILogger<PasswordService> logger;
    private AuthenticationSettings settings;
    public PasswordService(IOptionsMonitor<AuthenticationSettings> authenticationSettingOptions, ILogger<PasswordService> logger)
    {
        settings = authenticationSettingOptions.CurrentValue;
        this.logger = logger;
    }

    public string GetPasswordHash(string password)
    {
        var keyInBytes = Encoding.UTF8.GetBytes(settings.HashKey);
        using var hasher = new System.Security.Cryptography.HMACSHA512(keyInBytes);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashBytes = hasher.ComputeHash(passwordBytes);

        var hashString = Convert.ToBase64String(hashBytes);

        return hashString;
    }

    public bool IsValidPassword(string password, string passwordHash)
    {
        var currentPasswordHash = GetPasswordHash(password);

        return currentPasswordHash == passwordHash;
    }
}
