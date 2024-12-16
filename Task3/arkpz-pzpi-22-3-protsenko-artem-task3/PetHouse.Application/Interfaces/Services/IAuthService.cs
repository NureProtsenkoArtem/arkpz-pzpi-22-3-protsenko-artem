using PetHouse.Application.Contracts.User;

namespace PetHouse.Application.Interfaces.Services;

public interface IAuthService
{
   Task Register(string Name, string Password, string Email);
   Task<LoginUserResponse> Login(string Name, string Password);
   Task VerifyEmail(string email, string verificationCode);
}