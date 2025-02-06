using DotNetNlayer.Core.Services;
using DotnetNlayer.Service;
using DotnetNlayer.Service.Services;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class EmailServices
{
    public static void AddEmailConfirmationServices(this IServiceCollection services,IConfiguration configuration)
    {
        var confirmEmail = configuration.GetValue<bool>("ConfirmEmail");

        if (confirmEmail)
        {
            services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
        }
        else
        {
            services.AddScoped<IEmailConfirmationService,DummyEmailConfirmationService>();
        }
    }

    

}