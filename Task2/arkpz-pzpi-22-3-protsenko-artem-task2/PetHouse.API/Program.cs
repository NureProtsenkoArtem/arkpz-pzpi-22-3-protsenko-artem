using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHouse.API.Exstensions;
using PetHouse.API.Helpers;
using PetHouse.Application.Interfaces;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Application.Services;
using PetHouse.Core.Models;
using PetHouse.Infrastructure.Interfaces;
using PetHouse.Infrastructure.Security;
using PetHouse.Infrastructure.Security.Jwt;
using PetHouse.Persistence;
using PetHouse.Persistence.Interfaces;
using PetHouse.Persistence.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

services.AddApiAuthentication(configuration);
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
   options.EnableAnnotations();
});

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IMealRepository, MealRepository>();

services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPetService, PetService>();
services.AddScoped<IDeviceService, DeviceService>();
services.AddScoped<IHealthAnalysisService, HealthAnalysisService>();
services.AddScoped<IMealService, MealService>();


services.AddScoped<IPasswordHasher, PasswordHasher>();
services.AddScoped<IJwtProvider, JwtProvider>();

services.AddDbContext<PetHouseDbContext>(options =>
{
   options.UseNpgsql(configuration.GetConnectionString("PetHouseDbContext"));
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
   app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();