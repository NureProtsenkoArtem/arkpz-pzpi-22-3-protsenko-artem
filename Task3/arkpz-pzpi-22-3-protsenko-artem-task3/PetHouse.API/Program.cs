using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetHouse.API.Contracts;
using PetHouse.API.Contracts.Storage;
using PetHouse.API.Exstensions;
using PetHouse.API.Helpers;
using PetHouse.Application.Contracts.Mail;
using PetHouse.Application.Interfaces;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Application.Services;
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

   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "Введіть токен у форматі 'Bearer {your JWT token}'"
   });

   options.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
      {
         new OpenApiSecurityScheme
         {
            Reference = new OpenApiReference
            {
               Type = ReferenceType.SecurityScheme,
               Id = "Bearer"
            }
         },
         Array.Empty<string>()
      }
   });
});

services.Configure<SenderSettings>(configuration.GetSection("SenderData"));
services.Configure<AwsOptions>(configuration.GetSection("AWS"));
services.AddSingleton<IAmazonS3>(AwsS3ClientFactory.CreateS3Client(configuration));

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IMealRepository, MealRepository>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IStorageService, StorageService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPetService, PetService>();
services.AddScoped<IDeviceService, DeviceService>();
services.AddScoped<IHealthAnalysisService, HealthAnalysisService>();
services.AddScoped<IMealService, MealService>();
services.AddScoped<IMailService, MailService>();
services.AddTransient<IPasswordService, PasswordService>();
services.AddTransient<IAdminService>(provider =>
{
   string? connectionString = configuration.GetConnectionString("PetHouseDbContext");
   return new AdminService(connectionString);
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();