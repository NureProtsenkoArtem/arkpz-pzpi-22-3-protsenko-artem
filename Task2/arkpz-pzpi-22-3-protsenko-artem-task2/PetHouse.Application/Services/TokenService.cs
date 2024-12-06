using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Models;
using PetHouse.Infrastructure.Interfaces;

namespace PetHouse.Application.Services;

public class TokenService(IJwtProvider jwtProvider) : ITokenService
{
   public Task<Tuple<string,string>> GenerateTokens(User user)
   {
      var accessToken = jwtProvider.GenerateAccessToken(user);
      var refreshToken = jwtProvider.GenerateRefreshToken(user);

      return Task.FromResult(Tuple.Create(accessToken, refreshToken));
   }
   
}