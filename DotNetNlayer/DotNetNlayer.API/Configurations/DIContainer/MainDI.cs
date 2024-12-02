using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.AdminServices;
using DotnetNlayer.Repository;
using DotnetNlayer.Repository.Repositories;
using DotnetNlayer.Repository.Repositories.AdminRepositories;
using DotnetNlayer.Service.Services;
using DotnetNlayer.Service.Services.AdminServices;
using SharedLibrary;

namespace DotNetNlayer.API.Configurations.DIContainer;

public static class MainDi
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppTokenOptions>(configuration.GetSection("TokenOptions"));
        services.Configure<List<ClientLoginDto>>(configuration.GetSection("Clients"));

        services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();

        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();
        services.AddScoped<IAdminRoleService, AdminRoleService>();
        services.AddScoped<IAdminUserRoleRepository, AdminUserRoleRepository>();
        services.AddScoped<IAdminUserRoleService, AdminUserRoleService>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}