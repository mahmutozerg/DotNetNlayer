using System.Reflection;
using DotNetNlayer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetNlayer.Repository;

public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
{
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
        
    }
    

}