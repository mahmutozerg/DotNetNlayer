using DotNetNlayer.Core.Constants;
using DotNetNlayer.Core.Models;
using Microsoft.AspNetCore.Identity;


namespace DotnetNlayer.Repository.Seed;
public static class RoleSeedData
{
    public static async Task InitializeAsync(RoleManager<AppRole> roleManager)
    {

        foreach (var role in RoleConstants.Roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new AppRole(role));
              
            }
        }
    }
}