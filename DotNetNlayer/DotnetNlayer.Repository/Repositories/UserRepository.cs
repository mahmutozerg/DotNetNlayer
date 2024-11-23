using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotnetNlayer.Repository.Repositories;

public class UserRepository:GenericRepository<AppUser>,IUserRepository
{
    private readonly DbSet<AppUser> _users;
    public UserRepository(AppDbContext context) : base(context)
    {
        _users = context.Set<AppUser>();
    }
}