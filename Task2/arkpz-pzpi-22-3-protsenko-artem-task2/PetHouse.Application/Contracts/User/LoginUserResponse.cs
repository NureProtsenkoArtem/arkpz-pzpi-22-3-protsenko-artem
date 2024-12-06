using System.Reflection.Metadata.Ecma335;

namespace PetHouse.Application.Contracts.User;

public record LoginUserResponse
{
   public string AccessToken { get; set; }
   public string RefreshToken { get; set; }
}