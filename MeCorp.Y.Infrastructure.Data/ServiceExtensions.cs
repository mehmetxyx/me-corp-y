using MeCorp.Y.Api;
using MeCorp.Y.Infrastructure.Data.Repositories;
using MeCorp.Y.Infrastructure.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeCorp.Y.Infrastructure.Data;

public static class ServiceExtensions
{
    public static void AddDataServices(this IServiceCollection services, IConfigurationManager connectionManager)
    {
        var connectionString = connectionManager.GetConnectionString("PostgresqlDb");
        //services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("mydb"));
        services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReferralTokenRepository, ReferralTokenRepository>();
        services.AddScoped<IBlockedIpRepository, BlockedIpRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
