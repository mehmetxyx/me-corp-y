using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeCorp.Y.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> logger;
    private AuthenticationSettings settings;
    public TokenService(IOptionsMonitor<AuthenticationSettings> authenticationSettingOptions, ILogger<TokenService> logger)
    {
        settings = authenticationSettingOptions.CurrentValue;
        this.logger = logger;
    }
    public string GenerateToken(GenerateTokenArguments arguments)
    {
        var token = string.Empty;
        try
        {
            var secretInBytes = Encoding.UTF8.GetBytes(settings.SecretKey);

            var symmetricSecurityKey = new SymmetricSecurityKey(secretInBytes);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var claimIdentity = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, arguments.UserId.ToString()),
                    new Claim(ClaimTypes.Name, arguments.Username),
                    new Claim(ClaimTypes.Role, arguments.Role)
                }
            );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = DateTime.UtcNow.Add(new TimeSpan(3, 0, 0, 0)),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Can not create jwt token");
        }

        return token;
    }
}
