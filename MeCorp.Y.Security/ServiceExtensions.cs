using Microsoft.AspNetCore.Authentication.JwtBearer;
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
}
