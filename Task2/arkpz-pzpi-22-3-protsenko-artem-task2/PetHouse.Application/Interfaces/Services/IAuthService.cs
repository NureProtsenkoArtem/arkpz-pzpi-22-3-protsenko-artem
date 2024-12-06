using PetHouse.Application.Contracts.User;

namespace PetHouse.Application.Interfaces;

public interface IAuthService
{
   Task Register(string Name, string Password, string Email);
   Task<LoginUserResponse> Login(string Name, string Password);
   
}