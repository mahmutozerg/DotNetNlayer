using System.Reflection;
using DotnetNlayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotNetNlayer.API.Configurations.DBContexts;

public static class AppDbContextConfiguration
{
    public static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AppDbContext>(x =>
        {
            x.UseSqlServer(configuration.GetConnectionString("SqlCon"), options =>
            {
                options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                options.EnableRetryOnFailure();
            });

            x.ConfigureWarnings(warning => warning.Log(RelationalEventId.PendingModelChangesWarning));
        });

    }
}