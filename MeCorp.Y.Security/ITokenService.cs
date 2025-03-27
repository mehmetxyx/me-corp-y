namespace MeCorp.Y.Infrastructure.Security;

public interface ITokenService
{
    string GenerateToken(GenerateTokenArguments generateTokenArguments);
}