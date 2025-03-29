using MeCorp.Y.Application;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Application.Services;
using MeCorp.Y.Domain.Enums;
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
    public async Task<ActionResult<RegisteredUserResponseDto>> Register(RegisteredUserRequestDto registeredUserRequestDto)
    {
        Result<RegisteredUserResponseDto> result = await authorizationService.CreateUserAsync(registeredUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);
        return Ok(result.Value);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto)
    {
        Result<LoginUserResponseDto> result = await authorizationService.LoginAsync(loginUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpGet("referral-tokens/{code}")]
    public async Task<ActionResult<GetReferralTokenResponse>> GetReferralToken(string code)
    {
        Result<GetReferralTokenResponse> result = await authorizationService.GetReferralTokenAsync(code);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpPost("referral-tokens")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CreateReferralTokenResponse>> CreateReferralToken(CreateReferralTokenRequest referralTokenRequest)
    {
        Result<CreateReferralTokenResponse> result = await authorizationService.CreateReferalTokenAsync(referralTokenRequest);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }
}
