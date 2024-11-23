using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Constants;

namespace DotnetNlayer.Repository.Seed;

public static class UserSeedData
{
    public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {

        foreach (var role in RoleConstants.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole(role));
            }
        }

        var adminEmail = SeedConstants.AdminEmail;
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
}
