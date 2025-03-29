using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Domain.DomainEntities;
using MeCorp.Y.Infrastructure.Data;
using MeCorp.Y.Infrastructure.Data.Repositories;
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

    public AuthService(
        ILogger<AuthService> logger,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IReferralTokenRepository referralTokenRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.userRepository = userRepository;
        this.passwordService = passwordService;
        this.tokenService = tokenService;
        this.referralTokenRepository = referralTokenRepository;
    }

    public async Task<Result<RegisteredUserResponseDto>> CreateUserAsync(RegisteredUserRequestDto userRequest)
    {
        try
        {
            Result<User> usersResult = await userRepository.GetUsersByUsername(userRequest.Username);
            if (usersResult.IsSuccessful)
                return new Result<RegisteredUserResponseDto> { Message = $"User {userRequest.Username} already exist" };

            if(userRequest.Role == Domain.Enums.UserRole.Admin)
            {
                Result<ReferralToken> referralResult = await referralTokenRepository.GetByCode(userRequest.ReferralCode);
                if(!referralResult.IsSuccessful || !referralResult.Value.IsValid)
                    return new Result<RegisteredUserResponseDto> { Message = $"Referral code is invalid!" };
            }

            string passwordHash = passwordService.GetPasswordHash(userRequest.Password);

            var user = new User
            {
                Username = userRequest.Username,
                Role = userRequest.Role,
                PasswordHash = passwordHash,
                CreatedAtUtc = DateTime.UtcNow
            };

            await userRepository.Add(user);
            await unitOfWork.SaveAsync();

            var createUserResult = await userRepository.GetUsersByUsername(userRequest.Username);

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

    public async Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginUserRequest)
    {
        try
        {
            Result<User> userResult = await userRepository.GetUsersByUsername(loginUserRequest.Username);
            if (!userResult.IsSuccessful)
                return new Result<LoginUserResponseDto> { Message = $"User {loginUserRequest.Username} doesn't exist!" };

            if(!passwordService.IsValidPassword(loginUserRequest.Password, userResult.Value.PasswordHash))
            {
                userResult.Value.IncreaseFailedLoginCount();

                await userRepository.Update(userResult.Value);
                unitOfWork.SaveAsync();

                if (userResult.Value.IsBlocked)
                    return new Result<LoginUserResponseDto> { Message = $"User {loginUserRequest.Username} blocked for too many failed logins!" };

                return new Result<LoginUserResponseDto> { Message = "Invalid password!" };
            }

            userResult.Value.ResetFailedLogins();
            userRepository.Update(userResult.Value);

            unitOfWork.SaveAsync();

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

            if (!result.IsSuccessful)
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
                return new Result<CreateReferralTokenResponse> { Message = $"User {referralTokenRequest.Code} already exist" };

            var referralToken = new ReferralToken
            {
                Code = referralTokenRequest.Code,
                IsValid = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            Result<ReferralToken> addReferralTokenResult = await referralTokenRepository.Add(referralToken);

            if (!addReferralTokenResult.IsSuccessful)
                return new Result<CreateReferralTokenResponse>
                {
                    Message = $"ReferralToken {addReferralTokenResult.Value.Code} cannot be created."
                };

            await unitOfWork.SaveAsync();

            return new Result<CreateReferralTokenResponse>
            {
                IsSuccessful = true,
                Value = new CreateReferralTokenResponse
                {
                    Id = addReferralTokenResult.Value.Id,
                    Code = addReferralTokenResult.Value.Code,
                    IsValid = addReferralTokenResult.Value.IsValid,
                    CreatedAtUtc = addReferralTokenResult.Value.CreatedAtUtc
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
