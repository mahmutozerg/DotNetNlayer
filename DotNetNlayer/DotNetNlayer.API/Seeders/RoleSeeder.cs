using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTO.Exceptions;

namespace DotNetNlayer.API.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var services = scope.ServiceProvider;
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            foreach (var role in RoleConstants.Roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new AppRole(role));
              
                }
            }        
        }
        catch (Exception ex)
        {
            throw new SomethingWentWrongException(nameof(AppRole), "Something went wrong while role seeding");
        }
    }
}