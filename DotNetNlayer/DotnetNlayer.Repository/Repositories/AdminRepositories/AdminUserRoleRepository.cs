using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetNlayer.Repository.Repositories.AdminRepositories;

public class AdminUserRoleRepository:GenericRepository<AppUser>,IAdminUserRoleRepository
{
    private readonly DbSet<AppUser> _users;
    private readonly UserManager<AppUser> _userManager;
    public AdminUserRoleRepository(AppDbContext context, UserManager<AppUser> userManager) : base(context)
    {
        _userManager = userManager;
        _users = context.Set<AppUser>();
    }

    public async Task<IList<AppUser>> GetUsersWithRolesAsync(string roleName)
    {
        return await _userManager.GetUsersInRoleAsync(roleName);
    }
}