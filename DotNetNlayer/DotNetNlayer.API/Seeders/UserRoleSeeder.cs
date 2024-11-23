using DotNetNlayer.Core.Models;
using DotnetNlayer.Repository.Seed;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTO.Exceptions;

namespace DotNetNlayer.API.Seeders;

public static class UserRoleSeeder
{
    public static async Task SeedRolesToUser(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var services = scope.ServiceProvider;
        
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            
            await UserSeedData.InitializeAsync(userManager, roleManager);

        }
        catch (Exception ex)
        {
            throw new SomethingWentWrongException(nameof(AppRole), "Something went wrong while role seeding");
        }
    }
}