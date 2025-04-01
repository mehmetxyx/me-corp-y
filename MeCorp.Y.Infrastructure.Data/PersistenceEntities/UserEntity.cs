using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Domain.Enums;

namespace MeCorp.Y.Infrastructure.Data.PersistenceEntities;

public class UserEntity
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public static class UserExtensions
{
    public static UserEntity ToEntity(this User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }

    public static User ToDomainEntity(this UserEntity userEntity)
    {
        return new User
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            PasswordHash = userEntity.PasswordHash,
            Role = userEntity.Role,
            CreatedAtUtc = userEntity.CreatedAtUtc
        };
    }
}