using PetHouse.Core.Models;

namespace PetHouse.Application.Interfaces.Services;

public interface ITokenService
{
   Task<Tuple<string, string>> GenerateTokens(User user);
}