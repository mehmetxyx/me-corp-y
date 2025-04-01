using MeCorp.Y.Api.Controllers;
using MeCorp.Y.Api.Models;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Application.Services;
using Xunit;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using MeCorp.Y.Shared;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace MeCorp.Y.Api.Tests;

public class AuthControllerTests
{
    private readonly AuthController authController;
    private readonly IAuthService authService;
    public AuthControllerTests()
    {
        authService = Substitute.For<IAuthService>();
        authController = new AuthController(authService);
    }
    [Fact]
    public async Task Register_WhenRegisterationIsSuccessful_ReturnsOk()
    {
        var userRequest = new RegisteredUserRequestDto
        {
            Username = "username1",
            Password = "password1",
        };

        var userResponse = new RegisteredUserResponseDto
        {
            Id = 1,
            CreatedAtUtc = DateTime.UtcNow,
            Role = Domain.Enums.UserRole.Manager,
            Username = "username1"
        };

        var apiResponse = new ApiResponse<RegisteredUserResponseDto>
        {
            IsSuccessful = true,
            Data = userResponse
        };

        authService.CreateUserAsync(userRequest)
            .Returns(new Result<RegisteredUserResponseDto>
            {
                IsSuccessful = true,
                Value = userResponse
            });

        var actionResult = await authController.Register(userRequest);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var valueResult = Assert.IsType<ApiResponse<RegisteredUserResponseDto>>(okResult.Value);

        Assert.Equal("username1", valueResult.Data.Username);
    }

    [Fact]
    public async Task Register_whenRegisterationFails_RetunsBadRequest()
    {
        var userRequest = new RegisteredUserRequestDto
        {
            Username = "username1",
            Password = "password1"
        };

        var apiResponse = new ApiResponse<RegisteredUserResponseDto>{};

        authService.CreateUserAsync(userRequest)
            .Returns(new Result<RegisteredUserResponseDto> { IsSuccessful = false });

        var actionResult = await authController.Register(userRequest);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task Login_WhenSuccessful_ReturnsOk()
    {
        var userRequest = new LoginUserRequestDto
        {
            Password = "password1",
            Username = "username"
        };

        var userResponse = new LoginUserResponseDto
        {
            Id = 1,
            Role = Domain.Enums.UserRole.Customer,
            Token = "token",
            Username = "username"
        };

        var ip = "127.0.0.1";

        authService.LoginAsync(userRequest, ip)
            .Returns(new Result<LoginUserResponseDto> {
                IsSuccessful = true,
                Value = userResponse
            });

        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse(ip);
        authController.ControllerContext.HttpContext = httpContext;

        var actionResult = await authController.Login(userRequest);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var valueResult = Assert.IsType<ApiResponse<LoginUserResponseDto>>(okResult.Value);
        
        Assert.Equal("username", valueResult.Data.Username);
    }

    [Fact]
    public async Task Login_WhenFails_ReturnsBadRequest()
    {
        var userRequest = new LoginUserRequestDto
        {
            Password = "password1",
            Username = "username"
        };

        var userResponse = new LoginUserResponseDto
        {
            Id = 1,
            Role = Domain.Enums.UserRole.Customer,
            Token = "token",
            Username = "username"
        };

        var ip = "127.0.0.1";

        authService.LoginAsync(userRequest, ip)
            .Returns(new Result<LoginUserResponseDto>
            {
                IsSuccessful = false
            });

        var httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse(ip);
        authController.ControllerContext.HttpContext = httpContext;

        var actionResult = await authController.Login(userRequest);

        var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetReferralToken_WhenExists_ReturnsOk()
    {
        var referralCode = "CreateAsManager";

        var referralTokenResponse = new GetReferralTokenResponse { Code = referralCode };

        authService.GetReferralTokenAsync(referralCode)
            .Returns(new Result<GetReferralTokenResponse> { 
                IsSuccessful = true, 
                Value = referralTokenResponse 
            });

        var actionResult = await authController.GetReferralToken(referralCode);

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetReferralToken_WhenDoesNotExist_ReturnsBadRequest()
    {
        var referralCode = "CreateAsManager";

        var referralTokenResponse = new GetReferralTokenResponse {};

        authService.GetReferralTokenAsync(referralCode)
            .Returns(new Result<GetReferralTokenResponse>
            {
                IsSuccessful = false,
                Value = referralTokenResponse
            });

        var actionResult = await authController.GetReferralToken(referralCode);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateReferralToken_WhenSuccessful_ReturnsOk()
    {
        var referralCode = "code1";
        var request = new CreateReferralTokenRequest { Code = referralCode };

        var response = new CreateReferralTokenResponse
        {
            Id = 1,
            Code = referralCode,
            IsValid = true
        };

        authService.CreateReferalTokenAsync(request)
            .Returns(new Result<CreateReferralTokenResponse>
            {
                IsSuccessful = true,
                Value = response
            });
        
        var actionResult = await authController.CreateReferralToken(request);

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateReferralToken_WhenFails_ReturnsBadRequestk()
    {
        var referralCode = "code1";
        var request = new CreateReferralTokenRequest { Code = referralCode };

        authService.CreateReferalTokenAsync(request)
            .Returns(new Result<CreateReferralTokenResponse>
            {
                IsSuccessful = false
            });

        var actionResult = await authController.CreateReferralToken(request);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }
}
