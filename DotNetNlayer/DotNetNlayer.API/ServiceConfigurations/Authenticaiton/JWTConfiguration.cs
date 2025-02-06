using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using DotNetNlayer.Core.Configurations;
using DotnetNlayer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DotNetNlayer.API.ServiceConfigurations.Authenticaiton;


public static class JwtConfiguration
{
    public static void AddJwt(this IServiceCollection services, [NotNull]AppTokenOptions tokenOptions)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = tokenOptions.Issuer,
                ValidateIssuer = true,
                IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
                ValidAudience = tokenOptions.Audience[0],
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RoleClaimType = ClaimTypes.Role,
        
            };
        });

    }
}