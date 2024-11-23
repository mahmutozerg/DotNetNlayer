using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetNlayer.Repository.Repositories.AdminRepositories;

public class AdminUserRepository:GenericRepository<AppUser>,IAdminUserRepository
{
    private readonly DbSet<AppUser> _users;


    public AdminUserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
    }
}