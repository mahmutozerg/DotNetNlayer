using DotNetNlayer.Core.DTO.Manager;
using DotNetNlayer.Core.Services.Manager;
using DotnetNlayer.Service.Services.ManagerServices;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class HangfireJobDiConfiguration
{
    public static void AddHangfireRelatedServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IDatabaseBackupJobService, DatabaseBackupJobService>();
        services.Configure<DatabaseBackupJobOptionsDto>(configuration.GetSection("DatabaseBackupOptions"));

        
    }
}