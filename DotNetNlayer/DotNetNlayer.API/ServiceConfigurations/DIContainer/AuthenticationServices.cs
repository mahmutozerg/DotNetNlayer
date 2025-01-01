using DotNetNlayer.Core.Services;
using DotnetNlayer.Service.Services;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class AuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();

    }

}