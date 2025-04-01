using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Shared;
namespace MeCorp.Y.Application.Services;

public interface IAuthService
{
    Task<Result<RegisteredUserResponseDto>> CreateUserAsync(RegisteredUserRequestDto registeredUserRequestDto);
    Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginUserRequestDto, string? userIp);
    Task<Result<GetReferralTokenResponse>> GetReferralTokenAsync(string code);
    Task<Result<CreateReferralTokenResponse>> CreateReferalTokenAsync(CreateReferralTokenRequest referralTokenRequest);
}