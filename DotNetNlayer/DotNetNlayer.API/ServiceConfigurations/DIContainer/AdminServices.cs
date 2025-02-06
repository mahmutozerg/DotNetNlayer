using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services.AdminServices;
using DotnetNlayer.Repository.Repositories.AdminRepositories;
using DotnetNlayer.Service.Services.AdminServices;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class AdminServices
{

    public static void AddAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();
        services.AddScoped<IAdminRoleService, AdminRoleService>();
        services.AddScoped<IAdminUserRoleRepository, AdminUserRoleRepository>();
        services.AddScoped<IAdminUserRoleService, AdminUserRoleService>();
    }


}