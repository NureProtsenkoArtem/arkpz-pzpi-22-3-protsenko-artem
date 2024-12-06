using PetHouse.Application.Helpers;
using PetHouse.Application.Interfaces.Services;
using PetHouse.Core.Enums.User;
using PetHouse.Core.Models;
using PetHouse.Persistence.Interfaces;

namespace PetHouse.Application.Services;

public class UserService : GenericService<User>, IUserService
{
   private readonly IUnitOfWork _unitOfWork;
   public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }

   public async Task<User> UpdateUser(Guid userId, string name, string email, Role userRole)
   {
      var user = await Repository.FindById(userId);
      if (user == null)
      {
         throw new ApiException($"User with ID {userId} not found.",404);
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
}