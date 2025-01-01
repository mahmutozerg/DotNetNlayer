using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Services;
using DotnetNlayer.Repository.Repositories;
using DotnetNlayer.Service.Services;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class AppUserServices
{

    public static void AddAppUserServices(this IServiceCollection services)
    {
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IAppUserService, AppUserService>();
    }

}