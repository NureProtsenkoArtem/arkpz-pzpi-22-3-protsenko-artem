using PetHouse.Application.Contracts.User;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;
using PetHouse.Core.Models;
using PetHouse.Infrastructure.Interfaces;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class AuthService : IAuthService
{
   private readonly IUserRepository _userRepository;
   private readonly IPasswordHasher _passwordHasher;
   private readonly ITokenService _tokenService;

   public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
      ITokenService tokenService)
   {
      _userRepository = userRepository;
      _passwordHasher = passwordHasher;
      _tokenService = tokenService;
   }

   public async Task Register(string name, string password, string email)
   {
      var candidate = await _userRepository.GetByEmail(email);

      if (candidate != null)
      {
         throw new ApiException($"Email {email} is already in use",400);
      }

      var passwordHash = _passwordHasher.Generate(password);

      var user = new User
      {
         Email = email,
         Name = name,
         CreatedAt = DateTime.UtcNow,
         IsVerified = false,
         Password = passwordHash,
         UserId = Guid.NewGuid(),
         UserRole = Role.User,
      };

      await _userRepository.Add(user);
   }

   public async Task<LoginUserResponse> Login(string email, string password)
   {
      var loginUserResult = new LoginUserResponse();

      var candidate = await _userRepository.GetByEmail(email);

      if (candidate == null)
      {
         throw new ApiException($"User with email {email} wasn't found",404);
      }

      var passwordVerifyResult = _passwordHasher.Verify(password, candidate.Password);

      if (!passwordVerifyResult)
      {
         throw new ApiException("Incorrect password",400);
      }

      var (accessToken, refreshToken) = await _tokenService.GenerateTokens(candidate);

      loginUserResult.AccessToken = accessToken;
      loginUserResult.RefreshToken = refreshToken;

      return loginUserResult;
   }
   
}