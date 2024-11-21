using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // Enable user secrets
    builder.Configuration.AddUserSecrets<Program>();
}

// gelecekte core layerde bir Interface oluştur içinde key adlı değişken bulunduracak
// diğer katmanlara DI ile ata
var mySecretValue = builder.Configuration["MySecretKey"];

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi();
}



app.Run();