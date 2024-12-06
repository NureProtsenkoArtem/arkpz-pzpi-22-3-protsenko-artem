using System.Globalization;
using System.Security.Claims;
using PetHouse.Core.Models;

namespace PetHouse.Infrastructure.Interfaces;

public interface IJwtProvider
{
   string GenerateAccessToken(User user);
   string GenerateRefreshToken(User user);
   ClaimsPrincipal GetPrincipals(string refreshToken);
}