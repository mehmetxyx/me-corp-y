using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MeCorp.Y.Infrastructure.Security;

public static class ServiceExtensions
{
    public static void AddSecurityServices(this IServiceCollection services, IConfigurationManager configurationManager)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.Configure<AuthenticationSettings>(configurationManager.GetSection(nameof(AuthenticationSettings)));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                var settings = configurationManager.GetSection("AuthenticationSettings")
                .Get<AuthenticationSettings>();

                if(settings is null)
                {
                    Console.WriteLine("AuthenticationSettings cant be found");
                    return;
                }

                var securityKeyInBytes = Encoding.UTF8.GetBytes(settings.SecretKey);
                var symmetricSecurityKey = new SymmetricSecurityKey(securityKeyInBytes);

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = settings.ValidateIssuerSigningKey,
                    ValidateAudience = settings.ValidateAudience,
                    ValidAudience = settings.Audience,
                    ValidateIssuer = settings.ValidateIssuer,
                    ValidIssuer = settings.Issuer,
                    IssuerSigningKey = symmetricSecurityKey
                };
            });

        services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("AllowWeb", corsPolicyBuilder =>
            {
                corsPolicyBuilder
                .WithOrigins("http://localhost:5218")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    public static void AddRateLimiterPolicies(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOptions.AddFixedWindowLimiter("FixedWindowPolicy", rateLimiterOptions =>
            {
                //for testing purposes
                rateLimiterOptions.QueueLimit = 2;
                rateLimiterOptions.PermitLimit = 5;
                rateLimiterOptions.Window = TimeSpan.FromSeconds(1);
                rateLimiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            });
        });
    }

    public static void UseSecurityServices(this IApplicationBuilder applicationBuilder)
    {
        //applicationBuilder.UseHttpsRedirection();
        applicationBuilder.UseRateLimiter();
        applicationBuilder.UseCors("AllowWeb");
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
    }
}
