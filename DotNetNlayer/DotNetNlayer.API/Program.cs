using DotNetNlayer.API.Middleware;
using DotNetNlayer.API.Seeders;
using DotNetNlayer.API.ServiceConfigurations;
using DotNetNlayer.API.ServiceConfigurations.Authenticaiton;
using DotNetNlayer.API.ServiceConfigurations.DBContexts;
using DotNetNlayer.API.ServiceConfigurations.DIContainer;
using DotNetNlayer.BackgroundJob;
using DotNetNlayer.BackgroundJob.Schedules;
using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Manager;
using Scalar.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Enable user secrets
    builder.Configuration.AddUserSecrets<Program>();
}


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

builder.Services.AddAuthenticationServices();
builder.Services.AddAppUserServices();
builder.Services.AddAdminServices();
builder.Services.AddEmailConfirmationServices(builder.Configuration);

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddHangFireAsHostedService(builder.Configuration);
builder.Services.AddJwt(tokenOptions);
builder.Services.AddHangfireRelatedRepoServices(builder.Configuration);

builder.Services.Configure<DatabaseBackupJobOptionsDto>(builder.Configuration.GetSection("DatabaseBackupOptions"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();


builder.Services.AddLogging(o =>
{
    o.AddConsole();
    o.AddDebug();
});

var app = builder.Build();
await RoleSeeder.SeedRoles(app.Services);
await UserRoleSeeder.SeedRolesToUser(app.Services);


if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi();
}
var options = new RewriteOptions()
    .AddRedirect("^$", "scalar/v1");  // Redirect root to /scalar/v1
app.UseRewriter(options);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.UseExceptionHandlingMiddleware();
app.UseHangfireDashboard("/jobs");

DatabaseBackupSchedule.SetupDatabaseBackupJob();

JobRetriever.GetAllRecurringJobs();
await app.RunAsync();