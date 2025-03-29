using MeCorp.Y.Application.ApplicationServices;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MeCorp.Y.Api.Controllers;

[Route("api/users")]
[ApiController]
[EnableRateLimiting("FixedWindowPolicy")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserResponse>> GetUser(int id)
    {
        Result<GetUserResponse> result = await userService.GetUserById(id);

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }
    
    [HttpGet("admin-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GetAdminSummaryResponse>> GetAdminSummary()
    {
        Result<List<GetAdminSummaryResponse>> result = await userService.GetAdminSummary();

        if (!result.IsSuccessful)
            return BadRequest(result.Message);

        return Ok(result.Value);
    }
}
