using System.Reflection;
using System.Security.Claims;
using DotNetNlayer.API.Middleware;
using DotNetNlayer.API.Seeders;
using DotNetNlayer.Core.Configurations;
using DotNetNlayer.Core.DTO.Client;
using DotNetNlayer.Core.Models;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Repositories.AdminRepositories;
using DotNetNlayer.Core.Services;
using DotNetNlayer.Core.Services.AdminServices;
using DotnetNlayer.Repository;
using DotnetNlayer.Repository.Repositories;
using DotnetNlayer.Repository.Repositories.AdminRepositories;
using DotnetNlayer.Service.Services;
using DotnetNlayer.Service.Services.AdminServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Enable user secrets
    builder.Configuration.AddUserSecrets<Program>();
}
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<AppTokenOptions>();

builder.Services.Configure<AppTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<ClientLoginDto>>(builder.Configuration.GetSection("Clients"));
builder.Services.AddScoped<IAppAuthenticationService, AppAuthenticationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAdminRoleRepository, AdminRoleRepository>();
builder.Services.AddScoped<IAdminRoleService,AdminRoleService>();
builder.Services.AddScoped<IAdminUserRoleRepository, AdminUserRoleRepository>();
builder.Services.AddScoped<IAdminUserRoleService, AdminUserRoleService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContextPool<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        options.EnableRetryOnFailure();
    });

    // This is for work around of the bug at  .NET 9.0.1
    // If you don't do this you can not set any seed data with Dynamic values eg=> Datetime.Now , Guid and hash values
    x.ConfigureWarnings(warning => warning.Log(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequiredLength = 4;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidateIssuer = true,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidAudience = tokenOptions.Audience[0],
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RoleClaimType = ClaimTypes.Role,
        
    };
});
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


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.UseExceptionHandlingMiddleware();

await app.RunAsync();