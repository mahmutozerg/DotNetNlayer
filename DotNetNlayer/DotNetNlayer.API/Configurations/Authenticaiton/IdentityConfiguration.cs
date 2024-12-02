using DotNetNlayer.Core.Models;
using DotnetNlayer.Repository;
using Microsoft.AspNetCore.Identity;

namespace DotNetNlayer.API.Configurations.Authenticaiton;

public static class IdentityConfiguration
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

    }
}