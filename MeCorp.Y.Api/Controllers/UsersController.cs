using MeCorp.Y.Api.Models;
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
    public async Task<ActionResult<ApiResponse<GetUserResponse>>> GetUser(int id)
    {
        Result<GetUserResponse> result = await userService.GetUserById(id);

        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<GetUserResponse>
            {
                Message = result.Message
            });

        return Ok(new ApiResponse<GetUserResponse>
        {
            IsSuccessful = true,
            Data = result.Value
        });
    }
    
    [HttpGet("admin-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<List<GetAdminSummaryResponse>>>> GetAdminSummary()
    {
        Result<GetAdminSummaryResponse> result = await userService.GetAdminSummary();
        
        if (!result.IsSuccessful)
            return BadRequest(new ApiResponse<GetAdminSummaryResponse>
            {
                Message = result.Message
            });

        return Ok(new ApiResponse<GetAdminSummaryResponse>
        {
            IsSuccessful = true,
            Data = result.Value
        });
    }
}
