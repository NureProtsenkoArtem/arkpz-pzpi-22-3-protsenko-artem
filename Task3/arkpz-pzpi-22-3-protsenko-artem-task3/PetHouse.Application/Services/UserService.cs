using PetHouse.Application.Contracts.Mail;
using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;
using PetHouse.Core.Models;
using PetHouse.Infrastructure.Interfaces;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class UserService : GenericService<User>, IUserService
{
   private readonly IUnitOfWork _unitOfWork;
   private readonly IUserRepository _userRepository;
   private readonly IPasswordService _passwordService;
   private readonly IMailService _mailService;
   private readonly IPasswordHasher _passwordHasher;

   public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository,
      IPasswordService passwordService, IMailService mailService,
      IPasswordHasher passwordHasher) :
      base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
      _userRepository = userRepository;
      _passwordService = passwordService;
      _mailService = mailService;
      _passwordHasher = passwordHasher;
   }

   public async Task<User> UpdateUser(Guid userId, string name, string email, Role userRole)
   {
      var user = await Repository.FindById(userId);
      if (user == null)
      {
         throw new ApiException($"User with ID {userId} not found.", 404);
      }

      if (!Enum.IsDefined(typeof(Role), userRole))
      {
         throw new ApiException("Invalid user role", 400);
      }

      user.Name = name;
      user.Email = email;
      user.UserRole = userRole;

      await Repository.Update(user);
      await _unitOfWork.SaveChangesAsync();

      return user;
   }

   public async Task ResetUserPassword(string email)
   {
      var candidate = await _userRepository.GetByEmail(email);

      if (candidate == null)
         throw new ApiException($"User with email ${email} wasn't found", 404);

      var newPassword = _passwordService.GeneratePassword();

      var passwordHash = _passwordHasher.Generate(newPassword);

      candidate.Password = passwordHash;

      await _userRepository.Update(candidate);

      await _mailService.SendEmail(email, newPassword, EmailSendType.PasswordRecovering);

      await _unitOfWork.SaveChangesAsync();
   }

   public async Task ChangeUserPassword(Guid userId, string oldPassword, string newPassword)
   {
      var candidate = await _userRepository.FindById(userId);

      if (candidate == null)
         throw new ApiException("User wasn't found", 404);

      var passwordVerificationResult = _passwordHasher.Verify(oldPassword, candidate.Password);

      if (!passwordVerificationResult)
         throw new ApiException("Incorrect password", 400);

      var newPasswordHash = _passwordHasher.Generate(newPassword);

      candidate.Password = newPasswordHash;

      await _userRepository.Update(candidate);
      await _unitOfWork.SaveChangesAsync();
   }
}