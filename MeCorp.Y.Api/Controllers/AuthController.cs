using MeCorp.Y.Api.Models;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Application.Services;
using MeCorp.Y.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MeCorp.Y.Api.Controllers;

[Route("api/auth")]
[ApiController]
[EnableRateLimiting("FixedWindowPolicy")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService authorizationService;

    public AuthController(IAuthService authService)
    {
        this.authorizationService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RegisteredUserResponseDto>>> Register(RegisteredUserRequestDto registeredUserRequestDto)
    {
        Result<RegisteredUserResponseDto> result = await authorizationService.CreateUserAsync(registeredUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<RegisteredUserResponseDto> { 
                Message = result.Message 
            });

        return Ok(new ApiResponse<RegisteredUserResponseDto> { 
            IsSuccessful = true, 
            Data = result.Value 
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginUserResponseDto>>> Login(LoginUserRequestDto loginUserRequestDto)
    {
        Result<LoginUserResponseDto> result = await authorizationService.LoginAsync(loginUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<LoginUserResponseDto>
            {
                Message = result.Message
            });

        return Ok(new ApiResponse<LoginUserResponseDto>
        {
            IsSuccessful = true,
            Data = result.Value
        });
    }

    [HttpGet("referral-tokens/{code}")]
    public async Task<ActionResult<ApiResponse<GetReferralTokenResponse>>> GetReferralToken(string code)
    {
        Result<GetReferralTokenResponse> result = await authorizationService.GetReferralTokenAsync(code);

        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<GetReferralTokenResponse>
            {
                Message = result.Message
            });

        return Ok(new ApiResponse<GetReferralTokenResponse>
        {
            IsSuccessful = true,
            Data = result.Value
        });
    }

    [HttpPost("referral-tokens")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CreateReferralTokenResponse>>> CreateReferralToken(CreateReferralTokenRequest referralTokenRequest)
    {
        Result<CreateReferralTokenResponse> result = await authorizationService.CreateReferalTokenAsync(referralTokenRequest);

        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<CreateReferralTokenResponse>
            {
                Message = result.Message
            });

        return Ok(new ApiResponse<CreateReferralTokenResponse>
        {
            IsSuccessful = true,
            Data = result.Value
        });
    }
}
