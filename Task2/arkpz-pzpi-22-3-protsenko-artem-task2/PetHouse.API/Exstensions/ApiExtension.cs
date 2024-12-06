using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetHouse.API.Helpers;
using PetHouse.Infrastructure.Security.Jwt;

namespace PetHouse.API.Exstensions;

public static class ApiExtension
{
   public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
   {
      var jwtOptions = new JwtOptions();
      configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
      services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

      services.AddAuthentication(options =>
         {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "google";
            options.DefaultChallengeScheme = "github";
         })
         .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
         {
            options.TokenValidationParameters = new TokenValidationParameters
            {
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ClockSkew = TimeSpan.Zero,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessSecretKey))
            };

            options.Events = new JwtBearerEvents
            {
               OnMessageReceived = context =>
               {
                  var authorizationHeader = context.Request.Headers["Authorization"].ToString();
                  if (!string.IsNullOrEmpty(authorizationHeader) &&
                      authorizationHeader.StartsWith("Bearer "))
                  {
                     context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
                  }
                  else
                  {
                     context.Token = context.Request.Cookies["refreshToken"];

                     if (string.IsNullOrEmpty(context.Token))
                     {
                        context.Token = context.Request.Headers["X-Refresh-Token"];
                     }
                  }

                  return Task.CompletedTask;
               },

               OnChallenge = context =>
               {
                  context.HandleResponse();
                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  context.Response.ContentType = "application/json";
                  return context.Response.WriteAsync("UnAuthorized");
               }
            };
         });
   }
}