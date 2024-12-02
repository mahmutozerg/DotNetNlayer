using System.Reflection;
using System.Security.Claims;
using DotNetNlayer.API.Configurations;
using DotNetNlayer.API.Configurations.Authenticaiton;
using DotNetNlayer.API.Configurations.DBContexts;
using DotNetNlayer.API.Configurations.DIContainer;
using DotNetNlayer.API.Middleware;
using DotNetNlayer.API.Seeders;
using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.Models;
using DotnetNlayer.Repository;
using DotnetNlayer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddHangFireAsHostedService(builder.Configuration);
builder.Services.AddJwt(tokenOptions);










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



await app.RunAsync();