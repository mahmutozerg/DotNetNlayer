using Hangfire;

namespace DotNetNlayer.API.Configurations;

public static class Hangfire
{
    public static void AddHangFireAsHostedService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(c => c
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireCon")));

        services.AddHangfireServer();

    }
}