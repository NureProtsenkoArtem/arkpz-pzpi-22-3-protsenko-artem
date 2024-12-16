using PetHouse.Application.Contracts.Mail;
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
   private readonly IMailService _mailService;

   public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
      ITokenService tokenService, IMailService mailService)
   {
      _userRepository = userRepository;
      _passwordHasher = passwordHasher;
      _tokenService = tokenService;
      _mailService = mailService;
   }

   // Registers a new user by creating a user object, hashing their password, and sending a verification email.
   public async Task Register(string name, string password, string email)
   {
      // Check if the email is already in use.
      var candidate = await _userRepository.GetByEmail(email);

      if (candidate != null)
      {
         throw new ApiException($"Email {email} is already in use", 400);
      }

      // Generate a password hash.
      var passwordHash = _passwordHasher.Generate(password);

      // Create a new user object with default properties and a verification code.
      var user = new User
      {
         Email = email,
         Name = name,
         CreatedAt = DateTime.UtcNow,
         IsVerified = false,
         VerificationCode = GenerateActivationCode(),
         Password = passwordHash,
         UserId = Guid.NewGuid(),
         UserRole = Role.User,
      };

      // Send a verification email to the user.
      await _mailService.SendEmail(user.Email, user.VerificationCode, EmailSendType.VerificationCode);

      // Add the user to the repository.
      await _userRepository.Add(user);
   }

   // Logs in a user by verifying their credentials, checking account verification, and generating tokens.
   public async Task<LoginUserResponse> Login(string email, string password)
   {
      var loginUserResult = new LoginUserResponse();

      // Retrieve user by email.
      var candidate = await _userRepository.GetByEmail(email);

      if (candidate == null)
      {
         throw new ApiException($"User with email {email} wasn't found", 404);
      }

      // Verify the provided password.
      var passwordVerifyResult = _passwordHasher.Verify(password, candidate.Password);

      if (!passwordVerifyResult)
      {
         throw new ApiException("Incorrect password", 400);
      }

      // Ensure the user has verified their email.
      if (!candidate.IsVerified)
      {
         throw new ApiException("User doesn't verify his account", 403);
      }

      // Generate access and refresh tokens for the user.
      var (accessToken, refreshToken) = await _tokenService.GenerateTokens(candidate);

      loginUserResult.AccessToken = accessToken;
      loginUserResult.RefreshToken = refreshToken;
      loginUserResult.User = candidate;

      return loginUserResult;
   }

   // Verifies a user's email using a verification code.
   public async Task VerifyEmail(string email, string verificationCode)
   {
      // Retrieve user by email.
      var candidate = await _userRepository.GetByEmail(email);

      if (candidate == null)
         throw new ApiException($"User with email {email} wasn't found", 404);

      // Check if the provided verification code matches.
      if (candidate.VerificationCode != verificationCode)
         throw new ApiException($"Invalid verification code", 400);

      // Mark the user as verified and clear the verification code.
      candidate.IsVerified = true;
      candidate.VerificationCode = null;

      await _userRepository.Update(candidate);
   }

   // Generates a random numeric activation code for email verification.
   private string GenerateActivationCode()
   {
      const int length = 6; // Length of the activation code.
      var random = new Random();
      var code = new char[length];

      for (int i = 0; i < length; i++)
      {
         code[i] = (char)('0' + random.Next(0, 10));
      }

      return new string(code);
   }
}