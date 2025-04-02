using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data.Queries;
using MeCorp.Y.Infrastructure.Data.Repositories;
using MeCorp.Y.Shared;
using Microsoft.Extensions.Logging;

namespace MeCorp.Y.Application.ApplicationServices;

public class UserService : IUserService
{
    private readonly ILogger<UserService> logger;
    private readonly IUserRepository userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        this.logger = logger;
        this.userRepository = userRepository;
    }

    public async Task<Result<GetUserResponse>> GetUserById(int id)
    {
        Result<User> result = await userRepository.GetUserById(id);

        if (result.IsSuccessful is false || result.Value is null)
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

    public async Task<Result<GetAdminSummaryResponse>> GetAdminSummary()
    {
        try
        {
            AdminSummaryResult result = await userRepository.GetAdminSummary();

            return new Result<GetAdminSummaryResponse>
            {
                IsSuccessful = true,
                Value = new GetAdminSummaryResponse
                {
                    RegisteredCustomerCount = result.CustomerCount,
                    RegisteredManagerCount = result.ManagerCount
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot get the admin summary.");
        }

        return new Result<GetAdminSummaryResponse> { Message = "Cannot get the admin summary." };
    }
}
