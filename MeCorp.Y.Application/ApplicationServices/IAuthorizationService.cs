using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Shared;
namespace MeCorp.Y.Application.Services;

public interface IAuthorizationService
{
    Task<Result<RegisteredUserResponseDto>> CreateUserAsync(RegisteredUserRequestDto registeredUserRequestDto);
    Task<Result<LoginUserResponseDto>> LoginAsync(LoginUserRequestDto loginUserRequestDto);
    Task<Result<GetReferralTokenResponse>> GetReferralTokenAsync(GetReferralTokenRequest referralTokenRequest);
    Task<Result<CreateReferralTokenResponse>> CreateReferalTokenAsync(CreateReferralTokenRequest referralTokenRequest);
}