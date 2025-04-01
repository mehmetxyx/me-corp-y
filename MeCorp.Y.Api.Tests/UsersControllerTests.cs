using MeCorp.Y.Api.Controllers;
using MeCorp.Y.Application.ApplicationServices;
using MeCorp.Y.Application.Dtos;
using MeCorp.Y.Shared;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace MeCorp.Y.Api.Tests;

public class UsersControllerTests
{
    private readonly IUserService userService;
    private readonly UsersController usersController;
    public UsersControllerTests()
    {
        userService = Substitute.For<IUserService>();
        usersController = new UsersController(userService);
    }

    [Fact]
    public async Task GetUser_WhenSuccessful_ReturnsOk()
    {
        var userId = 1;
        var response = new GetUserResponse
        {
            Id = userId,
            Role = Domain.Enums.UserRole.Customer,
            Username = "username"
        };

        userService.GetUserById(userId)
            .Returns(new Result<GetUserResponse>
            {
                IsSuccessful = true,
                Value = response
            });
        
        var actionResult = await usersController.GetUser(userId);

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetUser_WhenFail_ReturnsBadRequest()
    {
        var userId = 1;
        
        userService.GetUserById(userId)
            .Returns(new Result<GetUserResponse>
            {
                IsSuccessful = false
            });

        var actionResult = await usersController.GetUser(userId);

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetAdminSummary_WhenSuccess_ReturnsOk()
    {
        var response = new GetAdminSummaryResponse
        {
            RegisteredCustomerCount = 2,
            RegisteredManagerCount = 3
        };

        userService.GetAdminSummary()
            .Returns(new Result<GetAdminSummaryResponse>
            {
                IsSuccessful = true,
                Value = response
            });

        var actionResult = await usersController.GetAdminSummary();

        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetAdminSummary_WhenFails_ReturnsBadRequest()
    {
        userService.GetAdminSummary()
            .Returns(new Result<GetAdminSummaryResponse>
            {
                IsSuccessful = false
            });

        var actionResult = await usersController.GetAdminSummary();

        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }
}
