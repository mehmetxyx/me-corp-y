using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Domain.Enums;
using MeCorp.Y.Infrastructure.Data.Repositories;
using MeCorp.Y.Infrastructure.Data.UnitOfWorks;
using MeCorp.Y.Infrastructure.Security;
using MeCorp.Y.Shared;
using Microsoft.Extensions.Logging;

namespace MeCorp.Y.Application.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserRepository userRepository;
    private readonly IPasswordService passwordService;
    private readonly ITokenService tokenService;
    private readonly IReferralTokenRepository referralTokenRepository;
    private readonly IBlockedIpRepository blockedIpRepository;

    public AuthService(
        ILogger<AuthService> logger,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IReferralTokenRepository referralTokenRepository,
        IBlockedIpRepository blockedIpRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.userRepository = userRepository;
        this.passwordService = passwordService;
        this.tokenService = tokenService;
        this.referralTokenRepository = referralTokenRepository;
        this.blockedIpRepository = blockedIpRepository;
    }

    public async Task<Result<RegisteredUserResponseDto>> CreateUserAsync(RegisteredUserRequestDto userRequest)
    {
        try
        {
            Result<User> usersResult = await userRepository.GetUsersByUsername(userRequest.Username);
            if (usersResult.IsSuccessful)
                return new Result<RegisteredUserResponseDto> { Message = $"User {userRequest.Username} already exist" };

            UserRole userRole = UserRole.Customer;

            if(userRequest.ReferralCode is not null)
            {
                Result<ReferralToken> referralResult = await referralTokenRepository.GetByCode(userRequest.ReferralCode);
                if(referralResult.IsSuccessful is false || (referralResult.Value is not null && referralResult.Value.IsValid is false))
                    return new Result<RegisteredUserResponseDto> { Message = $"Referral code is invalid!" };

                userRole = UserRole.Manager;
            }

            string passwordHash = passwordService.GetPasswordHash(userRequest.Password);

            var user = new User
            {
                Username = userRequest.Username,
                Role = userRole,
                PasswordHash = passwordHash,
                CreatedAtUtc = DateTime.UtcNow
            };

            await userRepository.Add(user);
            await unitOfWork.SaveAsync();

            var createUserResult = await userRepository.GetUsersByUsername(userRequest.Username);
            if(createUserResult.IsSuccessful is false || createUserResult.Value is null)
                return new Result<RegisteredUserResponseDto> { Message = $"User {userRequest.Username} already exist!" };

            return new Result<RegisteredUserResponseDto>
            {
                IsSuccessful = true,
                Value = new RegisteredUserResponseDto
                {
                    Id = createUserResult.Value.Id,
                    Username = createUserResult.Value.Username,
                    Role = createUserResult.Value.Role,
                    CreatedAtUtc = createUserResult.Value.CreatedAtUtc
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot create user {username}", userRequest.Username);
        }

        return new Result<RegisteredUserResponseDto>
        {
            Message = $"User {userRequest.Username} cannot be created."
        };
    }

    public async Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginUserRequest, string? userIp)
    {
        try
        {
            Result<BlockedIp> blockedIpResult = await blockedIpRepository.GetBlockedIpByIpAddress(userIp);

            if (blockedIpResult.IsSuccessful && blockedIpResult.Value is not null && blockedIpResult.Value.IsBlocked)
                return new Result<LoginUserResponseDto> { Message = $"Ip address {userIp} blocked for too many failed logins!" };

            var blockedIp = blockedIpResult?.Value ?? new BlockedIp { 
                IpAddress = userIp, 
                CreatedAtUtc = DateTime.UtcNow
            };

            Result<User> userResult = await userRepository.GetUsersByUsername(loginUserRequest.Username);
            if (userResult.IsSuccessful is false || userResult.Value is null)
                return new Result<LoginUserResponseDto> { Message = $"User {loginUserRequest.Username} doesn't exist!" };

            if(!passwordService.IsValidPassword(loginUserRequest.Password, userResult.Value.PasswordHash))
            {
                blockedIp.IncreaseFailedLoginCount();

                if (blockedIp.Id == 0)
                    await blockedIpRepository.Add(blockedIp);
                else
                    await blockedIpRepository.Update(blockedIp);

                await unitOfWork.SaveAsync();

                return new Result<LoginUserResponseDto> { Message = "Invalid password!" };
            }

            var token = tokenService.GenerateToken(new GenerateTokenArguments
            {
                UserId = userResult.Value.Id,
                Username = userResult.Value.Username,
                Role = userResult.Value.Role.ToString()
            });

            return new Result<LoginUserResponseDto>
            {
                IsSuccessful = true,
                Value = new LoginUserResponseDto
                {
                    Id = userResult.Value.Id,
                    Username = userResult.Value.Username,
                    Role = userResult.Value.Role,
                    CreatedAtUtc = userResult.Value.CreatedAtUtc,
                    Token = token
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot login user {username}", loginUserRequest.Username);
        }

        return new Result<LoginUserResponseDto> { Message = "An error occured while validating password!" };
    }

    public async Task<Result<GetReferralTokenResponse>> GetReferralTokenAsync(string code)
    {
        try
        {
            Result<ReferralToken> result = await referralTokenRepository.GetByCode(code);

            if (result.IsSuccessful is false || result.Value is null)
            {
                logger.LogError("Cannot find referral token code {code}", code);
                return new Result<GetReferralTokenResponse> { Message = $"Cannot find referral token code {code}" };
            }

            return new Result<GetReferralTokenResponse>
            {
                IsSuccessful = true,
                Value = new GetReferralTokenResponse
                {
                    Id = result.Value.Id,
                    Code = result.Value.Code,
                    CreatedAtUtc = result.Value.CreatedAtUtc,
                    IsValid = result.Value.IsValid
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot find referral token code {code}", code);
        }

        return new Result<GetReferralTokenResponse> { Message = "An error occured while validating password!" };
    }


    public async Task<Result<CreateReferralTokenResponse>> CreateReferalTokenAsync(CreateReferralTokenRequest referralTokenRequest)
    {
        try
        {
            Result<ReferralToken> result = await referralTokenRepository.GetByCode(referralTokenRequest.Code);
            if (result.IsSuccessful)
                return new Result<CreateReferralTokenResponse> { Message = $"ReferralCode {referralTokenRequest.Code} already exist" };

            var referralToken = new ReferralToken
            {
                Code = referralTokenRequest.Code,
                IsValid = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            await referralTokenRepository.Add(referralToken);
            await unitOfWork.SaveAsync();

            Result<ReferralToken> addedReferralTokenResult = await referralTokenRepository.GetByCode(referralTokenRequest.Code);
            if (addedReferralTokenResult.IsSuccessful is false || addedReferralTokenResult.Value is null)
                return new Result<CreateReferralTokenResponse> { Message = $"ReferralCode {referralTokenRequest.Code} hasn't been saved" };

            return new Result<CreateReferralTokenResponse>
            {
                IsSuccessful = true,
                Value = new CreateReferralTokenResponse
                {
                    Id = addedReferralTokenResult.Value.Id,
                    Code = addedReferralTokenResult.Value.Code,
                    IsValid = addedReferralTokenResult.Value.IsValid,
                    CreatedAtUtc = addedReferralTokenResult.Value.CreatedAtUtc
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot create user {username}", referralTokenRequest.Code);
        }

        return new Result<CreateReferralTokenResponse>
        {
            Message = $"User {referralTokenRequest.Code} cannot be created."
        };
    }
}
