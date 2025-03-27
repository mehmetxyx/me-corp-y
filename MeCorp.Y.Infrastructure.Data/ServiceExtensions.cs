using MeCorp.Y.Api;
using MeCorp.Y.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeCorp.Y.Infrastructure.Data;

public static class ServiceExtensions
{
    public static void AddDataServices(this IServiceCollection services, IConfigurationManager connectionManager)
    {
        services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("mydb"));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReferralTokenRepository, ReferralTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
