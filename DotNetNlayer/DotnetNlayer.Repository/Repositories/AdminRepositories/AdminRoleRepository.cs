using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetNlayer.Repository.Repositories.AdminRepositories;

public class AdminRoleRepository:GenericRepository<AppRole>,IAdminRoleRepository
{
    private readonly DbSet<AppRole> _roles;
    public AdminRoleRepository(AppDbContext context) : base(context)
    {
        _roles = context.Set<AppRole>();
    }
    
    
    
}