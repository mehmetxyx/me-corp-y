using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var securityKey = configurationManager.GetSection("AuthenticationSettings:SecretKey").Value;
                var securityKeyInBytes = Encoding.UTF8.GetBytes(securityKey);
                var symmetricSecurityKey = new SymmetricSecurityKey(securityKeyInBytes);

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = symmetricSecurityKey
                };
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
}
