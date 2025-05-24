using Microsoft.AspNetCore.Identity;
using TDD.BusinessLogic;
using TDD.Infrastructure;
using TDD.Shared;
using TDD.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Adding DI
builder.Services
    .AddBusinessLogic()
    .AddInfrastructure(builder.Configuration)
    .AddShared();

//adding identity 
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
