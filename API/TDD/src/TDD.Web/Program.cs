// File: Program.cs
// Author: Richard Benny
// Purpose: Configures and starts the ASP.NET Core web application, including authentication, services, middleware, and database migration.
using Microsoft.AspNetCore.Identity;
using TDD.BusinessLogic;
using TDD.Infrastructure;
using TDD.Shared;
using TDD.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TDD.Web;
using TDD.Shared.Options;
using TDD.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateActor = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

string allowedOrigins = "_allowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: allowedOrigins,
        builder =>
        {
            builder.SetIsOriginAllowedToAllowWildcardSubdomains().WithOrigins(
                "https://localhost:4200",
                "https://localhost",
                "http://localhost:4200",
                "http://localhost"
             ).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();




// Add services to the container.

builder.Services.AddControllers();

//making jwtavailable
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//Adding DI
builder.Services
    .AddBusinessLogic()
    .AddInfrastructure(builder.Configuration)
    .AddShared()
    .AddWeb();

//adding identity 
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();  
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowedOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
