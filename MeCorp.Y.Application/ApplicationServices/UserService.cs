using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.Repositories;
using MeCorp.Y.Shared;

namespace MeCorp.Y.Application.ApplicationServices;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<Result<GetUserResponse>> GetUserById(int id)
    {
        Result<User> result = await userRepository.GetUserById(id);

        if (!result.IsSuccessful)
            return new Result<GetUserResponse> { Message = result.Message };

        return new Result<GetUserResponse>
        {
            IsSuccessful = true,
            Value = new GetUserResponse
            {
                Id = result.Value.Id,
                Username = result.Value.Username,
                Role = result.Value.Role,
                CreatedAtUtc = result.Value.CreatedAtUtc
            }
        };
    }

    public async Task<Result<List<GetAdminSummaryResponse>>> GetAdminSummary()
    {
        Result<List<User>> result = await userRepository.GetAdminSummary();

        if (!result.IsSuccessful)
            return new Result<List<GetAdminSummaryResponse>> { Message = result.Message };

        var users = result.Value.Select(u => 
            new GetAdminSummaryResponse {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                CreatedAtUtc = u.CreatedAtUtc
        })
        .ToList();

        return new Result<List<GetAdminSummaryResponse>>
        {
            IsSuccessful = true,
            Value = users
        };
    }
}
