namespace MeCorp.Y.Infrastructure.Security;

public interface IPasswordService
{
    string GetPasswordHash(string password);
    bool IsValidPassword(string? password, string? passwordHash);
}