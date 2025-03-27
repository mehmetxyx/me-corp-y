using MeCorp.Y.Application.ApplicationServices;
using MeCorp.Y.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeCorp.Y.Application;

public static class ServiceExtensions
{
    public static void AddApplicationLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
    }
}
