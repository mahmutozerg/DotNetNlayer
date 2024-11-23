using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotnetNlayer.Repository.Repositories;

public class AppUserRepository:GenericRepository<AppUser>,IAppUserRepository
{
    private readonly DbSet<AppUser> _users;
    public AppUserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
    }
}