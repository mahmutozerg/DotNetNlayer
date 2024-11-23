using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Constants;
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
            
            foreach (var role in RoleConstants.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole(role));
                }
            }

            const string adminEmail = SeedConstants.AdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = SeedConstants.AdminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true, // Consider email confirmed for seeding
                    CreatedBy = "SeedData",
                    UpdatedBy = "SeedData",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, SeedConstants.AdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            
            }
        }
        catch (Exception ex)
        {
            throw new SomethingWentWrongException(nameof(AppRole), "Something went wrong while role seeding");
        }
    }
}