using MeCorp.Y.Application;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Application.Services;
using MeCorp.Y.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MeCorp.Y.Api.Controllers;

[Route("api/auth")]
[ApiController]
[EnableRateLimiting("FixedWindowPolicy")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService authorizationService;

    public AuthorizationController(IAuthorizationService authorizationService)
    {
        this.authorizationService = authorizationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisteredUserResponseDto>> Register(RegisteredUserRequestDto registeredUserRequestDto)
    {
        Result<RegisteredUserResponseDto> result = await authorizationService.CreateUserAsync(registeredUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);
        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDto>> Login(LoginUserRequestDto loginUserRequestDto)
    {
        Result<LoginUserResponseDto> result = await authorizationService.LoginAsync(loginUserRequestDto);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpGet("referral-tokens")]
    public async Task<ActionResult<GetReferralTokenResponse>> GetReferralToken([FromQuery] GetReferralTokenRequest referralTokenRequest)
    {
        Result<GetReferralTokenResponse> result = await authorizationService.GetReferralTokenAsync(referralTokenRequest);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpPost("referral-tokens")]
    public async Task<ActionResult<CreateReferralTokenResponse>> CreateReferralToken(CreateReferralTokenRequest referralTokenRequest)
    {
        Result<CreateReferralTokenResponse> result = await authorizationService.CreateReferalTokenAsync(referralTokenRequest);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpGet("aa")]
    public ActionResult Get()
    {
        return Ok("asdf");
    }
}
