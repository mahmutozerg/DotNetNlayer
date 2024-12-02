using DotNetNlayer.Core.DTO.Manager;
using DotNetNlayer.Core.Services.Manager;
using DotnetNlayer.Service.Services.ManagerServices;

namespace DotNetNlayer.API.Configurations.DIContainer;

public static class HangfireJobDi
{
    public static void AddHangfireRelatedRepoServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IDatabaseBackupJobService, DatabaseBackupJobService>();
        services.Configure<DatabaseBackupJobOptionsDto>(configuration.GetSection("DatabaseBackupOptions"));

        
    }
}