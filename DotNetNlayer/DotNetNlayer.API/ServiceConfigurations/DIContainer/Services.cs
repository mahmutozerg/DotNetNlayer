using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.SMTPServices;
using DotnetNlayer.Repository;
using DotnetNlayer.Repository.Repositories;

using DotnetNlayer.Service.Services;
using DotnetNlayer.Service.Services.SMTPServices;
using SharedLibrary;

namespace DotNetNlayer.API.ServiceConfigurations.DIContainer;

public static class Services
{

    
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppTokenOptions>(configuration.GetSection("TokenOptions"));
        services.Configure<List<ClientLoginDto>>(configuration.GetSection("Clients"));
        
        
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));



        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddTransient<ISmtpService, SmtpService>();

        
    }
}