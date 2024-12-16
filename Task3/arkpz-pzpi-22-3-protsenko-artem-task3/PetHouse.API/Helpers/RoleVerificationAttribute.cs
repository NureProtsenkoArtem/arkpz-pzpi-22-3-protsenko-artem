using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;

namespace PetHouse.API.Helpers;

public class RoleVerificationAttribute : Attribute, IAuthorizationFilter
{
   private readonly Role _requiredRole;

   public RoleVerificationAttribute(Role requiredRole)
   {
      _requiredRole = requiredRole;
   }

   public void OnAuthorization(AuthorizationFilterContext context)
   {
      var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

      if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
      {
         context.Result = new UnauthorizedResult();
         return;
      }

      var token = authorizationHeader.Substring("Bearer ".Length).Trim();

      try
      {
         var jwtHandler = new JwtSecurityTokenHandler();

         if (!jwtHandler.CanReadToken(token))
         {
            context.Result = new UnauthorizedResult();
            return;
         }

         var jwtToken = jwtHandler.ReadJwtToken(token);
         
         var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
         
         if (roleClaim != _requiredRole.ToString())
         {
            context.Result = new ForbidResult();
            return;
         }
      }
      catch (Exception)
      {
         context.Result = new UnauthorizedResult();
         return;
      }
   }
}